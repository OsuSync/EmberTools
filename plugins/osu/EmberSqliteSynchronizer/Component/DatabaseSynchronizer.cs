using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberSqliteSynchronizer.Models;
using EmberSqliteSynchronizer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OsuSqliteDatabase.Database;
using OsuSqliteDatabase.Model;
using OsuSqliteDatabase.Utils.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmberSqliteSynchronizer.Component
{
    public class DatabaseSynchronizer : IComponent, IEventHandler<OsuProcessMatchedEvent>, IEventHandler<OsuProcessTerminatedEvent>
    {
        private OsuDatabaseContext Db { get; }
        private ILogger<DatabaseSynchronizer> Logger { get; }
        private readonly SemaphoreSlim SynchronizeLock = new SemaphoreSlim(1);
        private readonly CancellationTokenSource cancellationSource = new CancellationTokenSource();
        private string _osuDbPath = string.Empty;
        public DatabaseSynchronizer(OsuDatabaseContext db, ILogger<DatabaseSynchronizer> logger)
        {
            Db = db;
            Logger = logger;
        }

        private const string osuDbFile = "osu!.db";
        public async ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            var dbPath = Path.Combine(@event.GameDirectory, osuDbFile);
            if (File.Exists(dbPath))
            {
                if (_osuDbPath == dbPath)
                {
                    return;
                }
                _osuDbPath = dbPath;
                Logger.LogInformation("Found osu! process and osu!.db, prepare to synchronize");
                if (SynchronizeLock.CurrentCount == 0) { return; }
                await SynchronizeLock.WaitAsync();
                try
                {
                    await MaintanceDatabase(dbPath, cancellationSource.Token);
                }
                finally
                {
                    SynchronizeLock.Release();
                }
            }
        }

        public async ValueTask Handle(OsuProcessTerminatedEvent @event)
        {
            await SynchronizeLock.WaitAsync();
            await Task.Delay(1000);
            try
            {
                await MaintanceDatabase(_osuDbPath, cancellationSource.Token);
            }
            finally
            {
                SynchronizeLock.Release();
            }
        }

        /// <summary>
        /// Create a new database by loaded osu!db content
        /// </summary>
        /// <param name="dbInfo"></param>
        /// <param name="beatmaps"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async ValueTask CreateDatabase(OsuDatabase dbInfo, List<OsuDatabaseBeatmap> beatmaps, CancellationToken token = default)
        {
            Logger.LogInformation("Creating database...");
            await Db.AddAsync(dbInfo);
            await Db.SaveChangesAsync();
            Logger.LogInformation($"Processing {dbInfo.BeatmapCount} beatmap...");
            DateTimeOffset startTime = DateTimeOffset.Now;

            Db.RemoveRange(Db.OsuDatabaseBeatmap);
            await Db.SaveChangesAsync();
            // Save all beatmaps
            foreach (var beatmap in beatmaps)
            {
                if (token.IsCancellationRequested) return;
                beatmap.OsuDatabaseId = dbInfo.Id;
                beatmap.OsuDatabaseTimings = null;
                beatmap.StarRatings = null;
            }

            // for performance, we save change every 5000 beatmaps
            foreach (var chunk in beatmaps.Chunk(5000))
            {
                if (token.IsCancellationRequested) return;
                await Db.AddRangeAsync(chunk);
                await Db.SaveChangesAsync();
                Logger.LogInformation($"Processed {chunk.Count} beatmap.");
            }
            Logger.LogInformation($"Create database complete in {(DateTimeOffset.Now - startTime).TotalMilliseconds}ms");
        }

        private async ValueTask UpdateDatabase(OsuDatabase currentInfo, OsuDatabase newInfo, List<OsuDatabaseBeatmap> beatmaps, CancellationToken token = default)
        {
            Logger.LogInformation("Updating database...");
            currentInfo.AccountLocked = newInfo.AccountLocked;
            currentInfo.BeatmapCount = newInfo.BeatmapCount;
            currentInfo.FolderCount = newInfo.FolderCount;
            currentInfo.Permission = newInfo.Permission;
            currentInfo.PlayerName = newInfo.PlayerName;
            currentInfo.UnlockedAt = newInfo.UnlockedAt;
            currentInfo.Version = newInfo.Version;
            Db.Update(currentInfo);
            await Db.SaveChangesAsync();

            // these properties temporarily move out of the comprasion fields
            var emitSet = new HashSet<string>()
            {
                nameof(OsuDatabaseBeatmap.Id),
                nameof(OsuDatabaseBeatmap.OsuDatabaseId),
                nameof(OsuDatabaseBeatmap.OsuDatabase),
                nameof(OsuDatabaseBeatmap.StarRatings),
                nameof(OsuDatabaseBeatmap.OsuDatabaseTimings),
                nameof(OsuDatabaseBeatmap.LatestModifiedAt),
                nameof(OsuDatabaseBeatmap.LatestPlayedAt),
                nameof(OsuDatabaseBeatmap.LatestUpdateAt),
            };
            var currentModifiedCount = 0;
            foreach (var beatmap in beatmaps)
            {
                if (token.IsCancellationRequested) return;
                // query exist record basic on FileName and FolderName
                var currentBeatmaps = await Db
                    .OsuDatabaseBeatmap
                    .Where((b) =>
                        (b.FileName == beatmap.FileName && b.FolderName == beatmap.FolderName)
                        || (b.BeatmapSetId > 0 && b.BeatmapId > 0
                            && b.BeatmapSetId == beatmap.BeatmapSetId && b.BeatmapId == beatmap.BeatmapId))
                    .ToListAsync();

                // no records that exist, save to database directly
                if (currentBeatmaps.Count == 0)
                {
                    beatmap.OsuDatabaseId = currentInfo.Id;
                    Logger.LogInformation($"Found new beatmap {beatmap.Artist} - {beatmap.Title} ({beatmap.FolderName}\\{beatmap.FileName})");
                    await Db.AddAsync(beatmap);
                }
                // exist 1 record, update
                else if (currentBeatmaps.Count == 1)
                {
                    var currentBeatmap = currentBeatmaps[0];
                    if (Db.Entry(currentBeatmap).Update(beatmap, emitSet) > 0)
                    {
                        Logger.LogInformation($"Update beatmap {beatmap.Artist} - {beatmap.Title} ({beatmap.FolderName}\\{beatmap.FileName})");
                    }
                }
                // exist more than one record, delete old records and create again
                else
                {
                    var corrupted = Db
                        .OsuDatabaseBeatmap
                        .Where((b) => b.FileName == beatmap.FileName && b.FolderName == beatmap.FolderName);
                    Db.OsuDatabaseBeatmap.RemoveRange(corrupted);
                    beatmap.OsuDatabaseId = currentInfo.Id;
                    await Db.AddAsync(beatmap);
                    Logger.LogInformation($"A corrupted data detected. Path = {beatmap.FolderName}\\{beatmap.FileName}");

                }

                if (++currentModifiedCount % 500 == 0)
                {
                    await Db.SaveChangesAsync();
                }
            }

            Logger.LogInformation($"Update complete.");
        }

        private async ValueTask MaintanceDatabase(string dbPath, CancellationToken token)
        {
            using var reader = new OsuDatabaseReader(dbPath);
            var newInfo = reader.ReadOsuDatabaseHead();


            // Do update or create operation depend on dbCount
            var dbCount = await Db.OsuDatabases.CountAsync();
            OsuDatabase currentInfo = null;
            if (dbCount > 0)
            {
                currentInfo = await Db.OsuDatabases.FirstAsync();
            }
            if (dbCount > 0 && currentInfo != null && currentInfo.BeatmapCount == newInfo.BeatmapCount)
            {
                Logger.LogInformation($"Database up-to-date.");
                return;
            }

            Logger.LogInformation("Beatmap count mismatch, start synchronize...");
            reader.ReadOsuDatabase(ref newInfo);

            var beatmaps = newInfo.Beatmaps;
            newInfo.Beatmaps = null;
            await Db.Database.AutoCommitTransactionScope(async () =>
            {
                // dbCount == 0, do create operation
                if (dbCount == 0)
                {
                    await CreateDatabase(newInfo, beatmaps, token);
                }
                // else do update operation
                else
                {
                    await UpdateDatabase(currentInfo, newInfo, beatmaps, token);
                }
                Logger.LogInformation($"Synchronize complete, {newInfo.BeatmapCount} saved...");
                // if token is cancelled, throw will let db operation rollback
                token.ThrowIfCancellationRequested();
                return true;
            });
        }


        public void Dispose()
        {
            cancellationSource.Cancel();
            cancellationSource.Dispose();
            SynchronizeLock.Dispose();
        }

    }
}
