using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeatmapDownloader.Abstract.Models
{
    public class BeatmapDownloaded : Event<BeatmapDownloaded>
    {
        public int BeatmapId { get; set; }
        public int BeatmapSetId { get; set; }
        public string DownloadProviderId { get; set; }
        public string DownloadProviderName { get; set; }
        public DateTime DownloadTime { get; set; }
    }
}
