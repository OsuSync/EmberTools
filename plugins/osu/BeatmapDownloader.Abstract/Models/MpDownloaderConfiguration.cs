using BeatmapDownloader.Abstract.Services.DownloadProvider;
using BeatmapDownloader.Abstract.Services.UI;
using EmberKernel.Services.UI.Mvvm.Dependency;

namespace BeatmapDownloader.Abstract.Models
{
    public class MpDownloaderConfiguration
    {
        [DependencyProperty(Name = nameof(DownloadNoVideo))]
        public bool DownloadNoVideo { get; set; } = true;

        [DependencyProperty(Name = nameof(DownloadProvider))]
        public DownloadProvider DownloadProvider { get; set; } = new DownloadProvider()
        {
            Id = typeof(BloodcatDownloadProvider).Name,
            Name = typeof(BloodcatDownloadProvider).GetProviderListDisplayName(),
        };

        [DependencyProperty(Name = nameof(AutoDownloadMultiplayerMissingBeatmap))]
        public bool AutoDownloadMultiplayerMissingBeatmap { get; set; } = true;
    }
}
