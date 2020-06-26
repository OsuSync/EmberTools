﻿using EmberKernel.Plugins.Components;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberSqliteSynchronizer.Component
{
    public class DatabaseSynchronizer : IComponent, IEventHandler<OsuProcessMatchedEvent>
    {
        private OsuDatabaseContext Db { get; }
        private ILogger<DatabaseSynchronizer> Logger { get; }
        private readonly SemaphoreSlim SynchronizeLock = new SemaphoreSlim(1);
        private readonly CancellationTokenSource cancellationSource = new CancellationTokenSource();
        private FileSystemWatcher _fsWatcher;
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
                _fsWatcher = new FileSystemWatcher(@event.GameDirectory)
                {
                    InternalBufferSize = 512,
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = true,
                };
                _fsWatcher.Changed += async(_, args) =>
                {
                    if (args.Name == osuDbFile || Path.GetDirectoryName(Path.GetDirectoryName(args.FullPath)) == @event.BeatmapDirectory)
                    {
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
                };
            }
        }

        private async ValueTask CreateDatabase(OsuDatabase dbInfo, List<OsuDatabaseBeatmap> beatmaps, CancellationToken token = default)
        {
            Logger.LogInformation("Creating database...");
            await Db.AddAsync(dbInfo);
            await Db.SaveChangesAsync();
            Logger.LogInformation($"Processing {dbInfo.BeatmapCount} beatmap...");

            Db.RemoveRange(Db.OsuDatabaseBeatmap);
            await Db.SaveChangesAsync();
            foreach (var beatmap in beatmaps)
            {
                if (token.IsCancellationRequested) return;
                beatmap.OsuDatabaseId = dbInfo.Id;
                beatmap.OsuDatabaseTimings = null;
                beatmap.StarRatings = null;
            }
            foreach (var chunk in beatmaps.Chunk(5000))
            {
                if (token.IsCancellationRequested) return;
                await Db.AddRangeAsync(chunk);
                await Db.SaveChangesAsync();
                Logger.LogInformation($"Processed {chunk.Count} beatmap.");
            }
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
                var currentBeatmaps = await Db
                    .OsuDatabaseBeatmap
                    .Where((b) => b.FileName == beatmap.FileName && b.FolderName == beatmap.FolderName)
                    .ToListAsync();
                if (currentBeatmaps.Count == 0)
                {
                    beatmap.OsuDatabaseId = currentInfo.Id;
                    Logger.LogInformation($"Found new beatmap {beatmap.Artist} - {beatmap.Title} ({beatmap.FolderName}\\{beatmap.FileName})");
                    await Db.AddAsync(beatmap);
                }
                else if (currentBeatmaps.Count == 1)
                {
                    var currentBeatmap = currentBeatmaps[0];
                    if (Db.Entry(currentBeatmap).Update(beatmap, emitSet) > 0)
                    {
                        Logger.LogInformation($"Update beatmap {beatmap.Artist} - {beatmap.Title} ({beatmap.FolderName}\\{beatmap.FileName})");
                    }
                }
                else
                {
                    var corrupted = Db
                        .OsuDatabaseBeatmap
                        .Where((b) => b.FileName == beatmap.FileName && b.FolderName == beatmap.FolderName);
                    Db.OsuDatabaseBeatmap.RemoveRange(corrupted);
                    await Db.AddAsync(beatmap);
                    Logger.LogInformation($"A corrupted data fixed");
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
                if (dbCount == 0)
                {
                    await CreateDatabase(newInfo, beatmaps, token);
                }
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
            _fsWatcher.Dispose();
        }

    }
}