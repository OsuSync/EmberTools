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
            builder.ConfigureDbContext<OsuDatabaseContext>();
            builder.ConfigureDbContext<BeatmapDownloaderDatabaseContext>();

            builder.UseConfigurationModel<MpDownloaderConfiguration>("MultiPlayerDownloaderUI");
            builder.ConfigureUIModel<MultiplayerDownloader, MpDownloaderConfiguration>();

            builder.ConfigureComponent<BeatmapDownloadService>().SingleInstance();

            builder.ConfigureDownloadProvider<SayobotDownloadProvider>();
            builder.ConfigureDownloadProvider<BloodcatDownloadProvider>();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Subscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Subscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            scope.AddDownloadProviderUIOptions<SayobotDownloadProvider>();
            scope.AddDownloadProviderUIOptions<BloodcatDownloadProvider>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Unsubscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            scope.RemoveDownloadProviderUIOptions<SayobotDownloadProvider>();
            scope.RemoveDownloadProviderUIOptions<BloodcatDownloadProvider>();
            return default;
        }
    }
}
