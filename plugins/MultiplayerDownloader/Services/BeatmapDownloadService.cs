using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiplayerDownloader.Models;
using MultiplayerDownloader.Services.DownloadProvider;
using OsuSqliteDatabase.Database;
using OsuSqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services
{
    public class BeatmapDownloadService : IComponent,
        IEventHandler<MultiplayerBeatmapIdInfo>,
        IEventHandler<OsuProcessMatchedEvent>
    {
        private ILifetimeScope Scope { get; }
        private ILogger<BeatmapDownloadService> Logger { get; }
        private OsuDatabaseContext Db { get; }
        private string OsuGamePath { get; set; } = string.Empty;
        private IPluginOptions<MultiplayerDownloader, MpDownloaderConfiguration> Options { get; }
        public BeatmapDownloadService(ILifetimeScope scope,
            ILogger<BeatmapDownloadService> logger,
            OsuDatabaseContext db,
            IPluginOptions<MultiplayerDownloader, MpDownloaderConfiguration> options)
        {
            Scope = scope;
            Logger = logger;
            Db = db;
            Options = options;
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
                var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider);

                var beatmapSetId = await downloadProvider.GetSetId(@event.BeatmapId);

                if (!beatmapSetId.HasValue)
                {
                    Logger.LogInformation($"Can't find beatmap ({@event.BeatmapId}) in download provider {options.DownloadProvider}");
                    return;
                }


                var hasBeatmap = await Db.OsuDatabaseBeatmap.AnyAsync(map => map.BeatmapSetId == beatmapSetId);
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

                await Db.OsuDatabaseBeatmap.AddAsync(new OsuDatabaseBeatmap()
                {
                    OsuDatabaseId = (await Db.OsuDatabases.FirstAsync()).Id,
                    BeatmapId = @event.BeatmapId,
                    BeatmapSetId = beatmapSetId.Value,
                });
                await Db.SaveChangesAsync();
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
