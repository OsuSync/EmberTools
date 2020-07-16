using EmberKernel.Services.EventBus;

namespace BeatmapDownloader.Abstract.Models.Events
{
    public class BeatmapDownloadTaskProgressUpdated : Event<BeatmapDownloadTaskProgressUpdated>
    {
        public DownloadBeatmapSetTask Task { get; set; }
        public int PercentCompleted { get; set; }
        public long BytesTotal { get; set; }
        public long BytesDownloaded { get; set; }
    }
}
