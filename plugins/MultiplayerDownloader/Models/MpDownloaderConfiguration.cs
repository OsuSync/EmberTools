using EmberKernel.Services.UI.Mvvm.Dependency;
using MultiplayerDownloader.Services.DownloadProvider;
using MultiplayerDownloader.Services.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerDownloader.Models
{
    public class MpDownloaderConfiguration
    {
        [DependencyProperty(Name = nameof(DownloadNoVideo))]
        public bool DownloadNoVideo { get; set; } = true;

        [DependencyProperty(Name = nameof(DownloadProvider))]
        public UIDownloadProdiver DownloadProvider { get; set; } = new UIDownloadProdiver()
        {
            Id = typeof(BloodcatDownloadProvider).Name,
            Name = typeof(BloodcatDownloadProvider).GetProviderListDisplayName(),
        };
    }
}
