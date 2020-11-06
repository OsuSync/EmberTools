using Autofac;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Models.Events;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.Database.Model;
using BeatmapDownloader.Extension;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Abstract.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using OsuSqliteDatabase.Database;
using OsuSqliteDatabase.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BeatmapDownloader.Services
{
    public class BeatmapDownloadService : IComponent,
        IEventHandler<OsuProcessMatchedEvent>,
        IEventHandler<BeatmapDownloadAddressPrepared>
    {
        private ILifetimeScope Scope { get; }
        private ILogger<BeatmapDownloadService> Logger { get; }
        private IEventBus EventBus { get; }
        private BeatmapDownloaderDatabaseContext DownloadDb { get; }
        private OsuDatabaseContext OsuDb { get; }
        private string OsuGamePath { get; set; } = string.Empty;
        private IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> OptionFactory { get; }
        public BeatmapDownloadService(ILifetimeScope scope,
            ILogger<BeatmapDownloadService> logger,
            BeatmapDownloaderDatabaseContext downloadDb,
            OsuDatabaseContext osuDb,
            IEventBus eventBus,
            IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> options)
        {
            Scope = scope;
            Logger = logger;
            DownloadDb = downloadDb;
            OsuDb = osuDb;
            OptionFactory = options;
            EventBus = eventBus;
        }

        private async ValueTask<EntityEntry<DownloadBeatmapSet>> CreateTask(BeatmapDownloadAddressPrepared @event, DownloadProvider provider)
        {
            var entity = await DownloadDb.AddAsync(new DownloadBeatmapSet()
            {
                BeatmapSetId = @event.BeatmapSetId,
                BeatmapId = @event.BeatmapId,
                DownloadProviderId = provider.Id,
                DownloadProviderName = provider.Name,
                CreatedAt = DateTime.Now,
            });
            await DownloadDb.SaveChangesAsync();
            return entity;
        }

        public async ValueTask Handle(BeatmapDownloadAddressPrepared @event)
        {
            var options = OptionFactory.Create();
            var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider.Id);

            // create download task in db
            var entity = await CreateTask(@event, options.DownloadProvider);
            var eventTask = entity.Entity.Cast<DownloadBeatmapSetTask>();
            entity.Entity.Status = DownloadStatus.NOT_STARTED;
            // start event
            EventBus.Publish(new BeatmapDownloadTaskStarted()
            {
                Task = eventTask,
            });
            Logger.LogInformation($"Start download #{@event.BeatmapSetId} from {@event.DownloadUrl}");

            var targetFile = Path.Combine(OsuGamePath, "Downloads", $"{@event.BeatmapSetId}.osz");
            entity.Entity.FullPath = eventTask.FullPath = targetFile;

            long prevDownloadedBytes = 0;
            // download notify events
            var progressEvent = new BeatmapDownloadTaskProgressUpdated()
            {
                Task = eventTask,
            };
            void handler(int percentage, long current, long total)
            {
                if (current - prevDownloadedBytes < 10240) return;

                prevDownloadedBytes = current;
                progressEvent.BytesTotal = total;
                progressEvent.BytesDownloaded = current;
                progressEvent.PercentCompleted = percentage;
                // download progress event
                EventBus.Publish(progressEvent);
            }
            downloadProvider.DownloadProgressChanged += handler;

            // start download
            entity.Entity.StartedAt = DateTime.Now;
            entity.Entity.Status = DownloadStatus.DOWNLOADING;
            var suggestFileName = await downloadProvider.Download(targetFile, @event.DownloadUrl);
            entity.Entity.CompletedAt = DateTime.Now;
            entity.Entity.Status = DownloadStatus.COMPLETED;

            Logger.LogInformation($"Beatmap #{@event.BeatmapSetId} downloaded");

            // adjust downloaded file name by response
            var suggestFilePath = Path.Combine(OsuGamePath, "Downloads", suggestFileName);
            if (suggestFileName != null && suggestFileName.Length > 0 && suggestFileName.EndsWith(".osz"))
            {
                File.Move(targetFile, suggestFilePath);
                targetFile = suggestFilePath;
            }
            entity.Entity.FullPath = eventTask.FullPath = targetFile;
            DownloadDb.Update(entity.Entity);
            await DownloadDb.SaveChangesAsync();

            await OsuDb.OsuDatabaseBeatmap.AddAsync(new OsuDatabaseBeatmap()
            {
                OsuDatabaseId = (await OsuDb.OsuDatabases.FirstAsync()).Id,
                BeatmapId = @event.BeatmapId,
                BeatmapSetId = @event.BeatmapSetId,
            });
            await OsuDb.SaveChangesAsync();

            // download completed event
            EventBus.Publish(new BeatmapDownloadTaskCompleted()
            {
                Task = eventTask,
            });

            if (options.AutoOpenDownloadedBeatmap)
            {
                try
                {
                    Logger.LogInformation($"Trying open beatmap {targetFile}");
                    Process.Start(new ProcessStartInfo(targetFile) { UseShellExecute = true });
                }
                catch
                {
                    Logger.LogWarning($"[Auto-Import] Failed to open .osz file, the file association not working.");
                }
            }
        }

        public ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            OsuGamePath = @event.GameDirectory;
            return default;
        }

        public void Dispose()
        {

        }
    }
}
