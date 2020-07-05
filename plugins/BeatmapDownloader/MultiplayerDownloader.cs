using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using BeatmapDownloader.Extension;
using BeatmapDownloader.Models;
using BeatmapDownloader.Services;
using OsuSqliteDatabase.Database;
using System;
using System.Threading.Tasks;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Database.Database;

namespace BeatmapDownloader
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Beatmap Downloader", Version = "1.0")]
    public class MultiplayerDownloader : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            // database
            builder.ConfigureDbContext<OsuDatabaseContext>();
            builder.ConfigureDbContext<BeatmapDownloaderDatabaseContext>();

            // configuration
            builder.UseConfigurationModel<MpDownloaderConfiguration>("MultiPlayerDownloaderUI");

            // service
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
            scope.Subscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Subscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            // add download provider options to UI
            await scope.AddDownloadProviderUIOptions<SayobotDownloadProvider>();
            await scope.AddDownloadProviderUIOptions<BloodcatDownloadProvider>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            // unsubscribe events
            scope.Unsubscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Unsubscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            // remove download provider options from UI
            await scope.RemoveDownloadProviderUIOptions<SayobotDownloadProvider>();
            await scope.RemoveDownloadProviderUIOptions<BloodcatDownloadProvider>();
        }
    }
}
