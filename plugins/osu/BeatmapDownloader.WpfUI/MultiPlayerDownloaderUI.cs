using Autofac;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.WpfUI.UI.View;
using BeatmapDownloader.WpfUI.UI.ViewModel;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using System.Threading.Tasks;

namespace BeatmapDownloader.WpfUI
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Beatmap Downloader UI", Version = "1.0")]
    public class MultiPlayerDownloaderUI : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            // Db context
            builder.ConfigureDbContext<BeatmapDownloaderDatabaseContext>();

            // configurations
            builder.UsePluginOptionsModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();

            // view models for UI
            builder.ConfigureComponent<DownloadProvidersViewModel>().SingleInstance();
            builder.ConfigureComponent<DownloadHistoryViewModel>().SingleInstance();

            // configuration UI
            builder.ConfigureUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();

            // UI
            builder.ConfigureUIComponent<MultiplayerDownloaderTab>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            // resolve ViewModel and bind to configuration UI
            var downloadProviderViewModel = scope.Resolve<DownloadProvidersViewModel>();
            scope.RegisterUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>(wpf => wpf
                .UseComboList(f => f.DownloadProvider, downloadProviderViewModel));

            // subscribe events
            scope.Subscription<DownloadingProcessChanged, DownloadHistoryViewModel>();
            scope.Subscription<BeatmapDownloaded, DownloadHistoryViewModel>();

            // initialize download tab
            await scope.InitializeUIComponent<MultiplayerDownloaderTab>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            // unsubscribe events
            scope.Unsubscription<BeatmapDownloaded, DownloadHistoryViewModel>();
            scope.Unsubscription<DownloadingProcessChanged, DownloadHistoryViewModel>();

            // uninitialize download tab
            await scope.UninitializeUIComponent<MultiplayerDownloaderTab>();

            // uninitialize configuration UI
            scope.UnregisterUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();
        }
    }
}
