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
        private IPluginOptions<MultiplayerDownloader, MpDownloaderConfiguration> Options { get; }
        public BeatmapDownloadService(ILifetimeScope scope,
            ILogger<BeatmapDownloadService> logger,
            OsuDatabaseContext osuDb,
            BeatmapDownloaderDatabaseContext downloadDb,
            IPluginOptions<MultiplayerDownloader, MpDownloaderConfiguration> options,
            IEventBus eventBus)
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
            // download beatmap
            try
            {
                var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider.Id) ;

                Logger.LogInformation($"Searching ({@event.BeatmapId}) in download provider {options.DownloadProvider}");
                var beatmapSetId = await downloadProvider.GetSetId(@event.BeatmapId);

                if (!beatmapSetId.HasValue)
                {
                    Logger.LogInformation($"Can't find beatmap ({@event.BeatmapId}) in download provider {options.DownloadProvider}");
                    return;
                }


                var hasBeatmap = await OsuDb.OsuDatabaseBeatmap.AnyAsync(map => map.BeatmapSetId == beatmapSetId);
                if (hasBeatmap)
                {
                    Logger.LogInformation($"HINT: Current beatmapset already created in your local osu!, downloader will not process this beatmapset");
                    return;
                }

                var url = await downloadProvider.GetBeatmapSetDownloadUrl(beatmapSetId.Value, options.DownloadNoVideo);
                var targetFile = Path.Combine(OsuGamePath, "Downloads", $"{beatmapSetId}.osz");

                Logger.LogInformation($"[Downloading] beatmap {@event.BeatmapId} (set = {beatmapSetId}) from ({options.DownloadProvider}) {url}");

                downloadProvider.DownloadProgressChanged += (percentage, current, total) =>
                {
                    Logger.LogInformation($"[Downloading] beatmapSet {beatmapSetId} {percentage}%, {current/1024}/{total/1024}");
                };

                var suggestFileName = await downloadProvider.Download(targetFile, url);
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
