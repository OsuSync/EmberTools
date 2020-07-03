using Autofac;
using EmberCore.KernelServices.UI.ViewModel.Configuration;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using EmberWpfCore.Components.Configuration.View.Component;
using MultiplayerDownloader.Extension;
using MultiplayerDownloader.Models;
using MultiplayerDownloader.Services;
using MultiplayerDownloader.Services.DownloadProvider;
using MultiplayerDownloader.Services.UI;
using OsuSqliteDatabase.Database;
using System;
using System.Threading.Tasks;

namespace MultiplayerDownloader
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Multiplayer Beatmap Auto Downloader", Version = "1.0")]
    public class MultiplayerDownloader : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureDbContext<OsuDatabaseContext>();

            builder.UsePluginOptionsModel<MultiplayerDownloader, MpDownloaderConfiguration>();
            builder.ConfigureUIModel<MultiplayerDownloader, MpDownloaderConfiguration>();

            builder.ConfigureComponent<BeatmapDownloadService>().SingleInstance();

            builder.ConfigureDownloadProvider<SayobotDownloadProvider>();
            builder.ConfigureDownloadProvider<BloodcatDownloadProvider>();

            builder.ConfigureComponent<DownloadProvidersViewModel>().SingleInstance();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            var downloadProviderViewModel = scope.Resolve<DownloadProvidersViewModel>();
            scope.RegisterUIModel<MultiplayerDownloader, MpDownloaderConfiguration>(wpf => wpf
                .UseComboList(f => f.DownloadProvider, downloadProviderViewModel));

            scope.Subscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Subscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            scope.AddDownloadProviderUIOptions<SayobotDownloadProvider>();
            scope.AddDownloadProviderUIOptions<BloodcatDownloadProvider>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.UnregisterUIModel<MultiplayerDownloader, MpDownloaderConfiguration>();

            scope.Unsubscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            scope.Unsubscription<OsuProcessMatchedEvent, BeatmapDownloadService>();

            scope.RemoveDownloadProviderUIOptions<SayobotDownloadProvider>();
            scope.RemoveDownloadProviderUIOptions<BloodcatDownloadProvider>();
            return default;
        }
    }
}
