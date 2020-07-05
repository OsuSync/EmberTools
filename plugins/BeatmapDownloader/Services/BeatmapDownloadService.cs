using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BeatmapDownloader.Models;
using OsuSqliteDatabase.Database;
using OsuSqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.Database.Model;

namespace BeatmapDownloader.Services
{
    public class BeatmapDownloadService : IComponent,
        IEventHandler<MultiplayerBeatmapIdInfo>,
        IEventHandler<OsuProcessMatchedEvent>
    {
        private ILifetimeScope Scope { get; }
        private ILogger<BeatmapDownloadService> Logger { get; }
        private IEventBus EventBus { get; }
        private OsuDatabaseContext OsuDb { get; }
        private BeatmapDownloaderDatabaseContext DownloadDb { get; }
        private string OsuGamePath { get; set; } = string.Empty;
        private IReadOnlyPluginOptions<MpDownloaderConfiguration> Options { get; }
        public BeatmapDownloadService(ILifetimeScope scope,
            ILogger<BeatmapDownloadService> logger,
            OsuDatabaseContext osuDb,
            BeatmapDownloaderDatabaseContext downloadDb,
            IEventBus eventBus,
            IReadOnlyPluginOptions<MpDownloaderConfiguration> options)
        {
            Scope = scope;
            Logger = logger;
            OsuDb = osuDb;
            DownloadDb = downloadDb;
            Options = options;
            EventBus = eventBus;
        }

        public ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            OsuGamePath = @event.GameDirectory;
            return default;
        }

        public async ValueTask Handle(MultiplayerBeatmapIdInfo @event)
        {
            if (OsuGamePath == string.Empty) return;
            if (!@event.HasValue) return;
            var options = Options.Create();
            if (!options.AutoDownloadMultiplayerMissingBeatmap) return;
            // download beatmap
            try
            {
                var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider.Id) ;

                Logger.LogInformation($"Searching ({@event.BeatmapId}) in download provider {options.DownloadProvider}");
                var downloadProgress = new DownloadingProcessChanged()
                {
                    ProviderName = options.DownloadProvider.Name,
                    SearchingBeatmap = true,
                    IsCompleted = false,
                    Idle = false,
                    BeatmapId = @event.BeatmapId,
                };
                EventBus.Publish(downloadProgress);
                var beatmapSetId = await downloadProvider.GetSetId(@event.BeatmapId);
                downloadProgress.SearchingBeatmap = false;
                EventBus.Publish(downloadProgress);
                if (!beatmapSetId.HasValue)
                {
                    Logger.LogInformation($"Can't find beatmap ({@event.BeatmapId}) in download provider {options.DownloadProvider}");
                    downloadProgress.Idle = true;
                    EventBus.Publish(downloadProgress);
                    return;
                }

                var hasBeatmap = await OsuDb.OsuDatabaseBeatmap.AnyAsync(map => map.BeatmapSetId == beatmapSetId);
                if (hasBeatmap)
                {
                    downloadProgress.Idle = true;
                    EventBus.Publish(downloadProgress);
                    Logger.LogInformation($"HINT: Current beatmapset already created in your local osu!, downloader will not process this beatmapset");
                    return;
                }

                var url = await downloadProvider.GetBeatmapSetDownloadUrl(beatmapSetId.Value, options.DownloadNoVideo);
                downloadProgress.SearchingBeatmap = false;
                downloadProgress.IsCompleted = false;
                EventBus.Publish(downloadProgress);
                var targetFile = Path.Combine(OsuGamePath, "Downloads", $"{beatmapSetId}.osz");

                Logger.LogInformation($"[Downloading] beatmap {@event.BeatmapId} (set = {beatmapSetId}) from ({options.DownloadProvider}) {url}");

                void handler(int percentage, long current, long total)
                {
                    Logger.LogInformation($"[Downloading] beatmapSet {beatmapSetId} {percentage}%, {current / 1024}/{total / 1024}");
                    downloadProgress.TotalKBytes = total / 1024;
                    downloadProgress.CurrentKBytes = current / 1024;
                    downloadProgress.Percentage = percentage;
                    EventBus.Publish(downloadProgress);
                }
                downloadProvider.DownloadProgressChanged += handler;

                var suggestFileName = await downloadProvider.Download(targetFile, url);
                downloadProvider.DownloadProgressChanged -= handler;
                downloadProgress.IsCompleted = true;
                downloadProgress.Idle = true;
                downloadProgress.Percentage = 0;
                EventBus.Publish(downloadProgress);


                var suggestFilePath = Path.Combine(OsuGamePath, "Downloads", suggestFileName);
                if (suggestFileName != null && suggestFileName.Length > 0 && suggestFileName.EndsWith(".osz"))
                {
                    File.Move(targetFile, suggestFilePath);
                    targetFile = suggestFilePath;
                }

                Logger.LogInformation($"[Complete] download to {targetFile}");
                try
                {
                    Process.Start(new ProcessStartInfo(targetFile) { UseShellExecute = true });
                }
                catch
                {
                    Logger.LogWarning($"[Auto-Import] Failed to open .osz file, the file association not working.");
                }

                await OsuDb.OsuDatabaseBeatmap.AddAsync(new OsuDatabaseBeatmap()
                {
                    OsuDatabaseId = (await OsuDb.OsuDatabases.FirstAsync()).Id,
                    BeatmapId = @event.BeatmapId,
                    BeatmapSetId = beatmapSetId.Value,
                });
                await OsuDb.SaveChangesAsync();

                await DownloadDb.AddAsync(new DownloadBeatmapSet()
                {
                    BeatmapSetId = beatmapSetId.Value,
                    BeatmapId = @event.BeatmapId,
                    DownloadProviderId = options.DownloadProvider.Id,
                    DownloadProviderName = options.DownloadProvider.Name,
                    DownloadTime = DateTime.Now,
                });
                await DownloadDb.SaveChangesAsync();

                EventBus.Publish(new BeatmapDownloaded()
                {
                    BeatmapSetId = beatmapSetId.Value,
                    BeatmapId = @event.BeatmapId,
                    DownloadProviderId = options.DownloadProvider.Id,
                    DownloadProviderName = options.DownloadProvider.Name,
                    DownloadTime = DateTime.Now,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Download Provider {options.DownloadProvider} repored an error");
            }

        }

        public void Dispose()
        {
        }

    }
}
