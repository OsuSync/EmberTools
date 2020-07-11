using Autofac;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Models.Events;
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
            builder.UsePluginOptionsModel<MultiPlayerDownloaderUI, BeatmapDownloaderConfiguration>();

            // view models for UI
            builder.ConfigureComponent<DownloadProvidersViewModel>().SingleInstance();
            builder.ConfigureComponent<DownloadHistoryViewModel>().SingleInstance();

            // configuration UI
            builder.ConfigureUIModel<MultiPlayerDownloaderUI, BeatmapDownloaderConfiguration>();

            // UI
            builder.ConfigureUIComponent<MultiplayerDownloaderTab>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            // resolve ViewModel and bind to configuration UI
            var downloadProviderViewModel = scope.Resolve<DownloadProvidersViewModel>();
            scope.RegisterUIModel<MultiPlayerDownloaderUI, BeatmapDownloaderConfiguration>(wpf => wpf
                .UseComboList(f => f.DownloadProvider, downloadProviderViewModel));

            // subscribe events
            scope.Subscription<BeatmapDownloadTaskStarted, DownloadHistoryViewModel>();
            scope.Subscription<BeatmapDownloadTaskProgressUpdated, DownloadHistoryViewModel>();
            scope.Subscription<BeatmapDownloadTaskCompleted, DownloadHistoryViewModel>();

            // initialize download tab
            await scope.InitializeUIComponent<MultiplayerDownloaderTab>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            // unsubscribe events
            scope.Unsubscription<BeatmapDownloadTaskStarted, DownloadHistoryViewModel>();
            scope.Unsubscription<BeatmapDownloadTaskProgressUpdated, DownloadHistoryViewModel>();
            scope.Unsubscription<BeatmapDownloadTaskCompleted, DownloadHistoryViewModel>();

            // uninitialize download tab
            await scope.UninitializeUIComponent<MultiplayerDownloaderTab>();

            // uninitialize configuration UI
            scope.UnregisterUIModel<MultiPlayerDownloaderUI, BeatmapDownloaderConfiguration>();
        }
    }
}
