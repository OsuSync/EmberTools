using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Models.Events;
using BeatmapDownloader.Models;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OsuSqliteDatabase.Database;
using System.Threading.Tasks;

namespace BeatmapDownloader.Services
{
    public class MultiplayerDownloadService : IComponent,
        IEventHandler<MultiplayerBeatmapIdInfo>
    {
        private IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> Options { get; }
        private BeatmapSearchService Search { get; }
        private ILogger<MultiplayerDownloadService> Logger { get; }
        private OsuDatabaseContext OsuDb { get; }
        private IEventBus EventBus { get; }
        public MultiplayerDownloadService(
            IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> options,
            BeatmapSearchService search,
            OsuDatabaseContext osuDb,
            ILogger<MultiplayerDownloadService> logger,
            IEventBus eventBus)
        {
            Options = options;
            Search = search;
            Logger = logger;
            OsuDb = osuDb;
            EventBus = eventBus;
        }

        public async ValueTask Handle(MultiplayerBeatmapIdInfo @event)
        {
            var options = Options.Create();
            if (!@event.HasValue) return;
            if (!options.AutoDownloadMultiplayerMissingBeatmap) return;

            var setId = await Search.SetId(@event.BeatmapId);
            if (!setId.HasValue)
            {
                Logger.LogInformation($"Beatmap set of beatmap #{@event.BeatmapId} is not found.");
                return;
            }

            var beatmapSetId = setId.Value;
            var hasBeatmap = await OsuDb.OsuDatabaseBeatmap.AnyAsync(map => map.BeatmapSetId == beatmapSetId);
            if (hasBeatmap)
            {
                Logger.LogInformation($"BeatmapSet #{beatmapSetId} already exist in your local, downloader will not process this beatmapSet");
                return;
            }

            Logger.LogInformation($"Preparing download BeatmapSet #{beatmapSetId}");
            var downloadUrl = await Search.DownloadAddressBySetId(beatmapSetId);
            EventBus.Publish(new BeatmapDownloadAddressPrepared()
            {
                BeatmapSetId = beatmapSetId,
                DownloadUrl = downloadUrl,
            });
        }

        public void Dispose()
        {
        }
    }
}
