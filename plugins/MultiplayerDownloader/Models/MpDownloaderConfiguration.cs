using EmberKernel.Services.UI.Mvvm.Dependency;
using MultiplayerDownloader.Services.DownloadProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerDownloader.Models
{
    public class MpDownloaderConfiguration
    {
        [DependencyProperty(Name = nameof(DownloadNoVideo))]
        public bool DownloadNoVideo { get; set; }

        [DependencyProperty(Name = nameof(DownloadProvider))]
        public string DownloadProvider { get; set; } = typeof(SayobotDownloadProvider).Name;
    }
}
