using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.WpfUI.UI.ViewModel;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using BeatmapDownloader.Abstract.Models;

namespace BeatmapDownloader.WpfUI
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Beatmap Downloader UI", Version = "1.0")]
    public class MultiPlayerDownloaderUI : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureDbContext<BeatmapDownloaderDatabaseContext>();
            builder.UsePluginOptionsModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();
            builder.ConfigureUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();

            builder.ConfigureComponent<DownloadProvidersViewModel>().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.MigrateDbContext<BeatmapDownloaderDatabaseContext>();
            var downloadProviderViewModel = scope.Resolve<DownloadProvidersViewModel>();

            scope.RegisterUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>(wpf => wpf
                .UseComboList(f => f.DownloadProvider, downloadProviderViewModel));
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.UnregisterUIModel<MultiPlayerDownloaderUI, MpDownloaderConfiguration>();
            return default;
        }
    }
}
