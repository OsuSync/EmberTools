using Autofac;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Models.Events;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.Extension;
using BeatmapDownloader.Models;
using BeatmapDownloader.Services;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using OsuSqliteDatabase.Database;
using System.Threading.Tasks;

namespace BeatmapDownloader
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Beatmap Downloader", Version = "1.0")]
    public class BeatmapDownloaderPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            // database
            builder.ConfigureDbContext<OsuDatabaseContext>();
            builder.ConfigureDbContext<BeatmapDownloaderDatabaseContext>();

            // configuration
            builder.UseConfigurationModel<BeatmapDownloaderConfiguration>("MultiPlayerDownloaderUI");

            // service
            builder.ConfigureComponent<BeatmapSearchService>().SingleInstance();
            builder.ConfigureComponent<MultiplayerDownloadService>().SingleInstance();
            builder.ConfigureComponent<BeatmapDownloadService>().SingleInstance();

            // download providers
            builder.ConfigureDownloadProvider<SayobotDownloadProvider>();
            builder.ConfigureDownloadProvider<BloodcatDownloadProvider>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            // migerate db
            await scope.MigrateDbContext<BeatmapDownloaderDatabaseContext>();

            // subscribe events
            scope.Subscription<MultiplayerBeatmapIdInfo, MultiplayerDownloadService>();
            scope.Subscription<BeatmapDownloadAddressPrepared, BeatmapDownloadService>();
            scope.Subscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            // add download provider options to UI
            await scope.AddDownloadProviderUIOptions<SayobotDownloadProvider>();
            await scope.AddDownloadProviderUIOptions<BloodcatDownloadProvider>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            // unsubscribe events
            scope.Unsubscription<MultiplayerBeatmapIdInfo, MultiplayerDownloadService>();
            scope.Unsubscription<BeatmapDownloadAddressPrepared, BeatmapDownloadService>();
            scope.Unsubscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            // remove download provider options from UI
            await scope.RemoveDownloadProviderUIOptions<SayobotDownloadProvider>();
            await scope.RemoveDownloadProviderUIOptions<BloodcatDownloadProvider>();
        }
    }
}
