using EmberKernel.Services.EventBus;

namespace BeatmapDownloader.Abstract.Models
{
    public class DownloadingProcessChanged : Event<DownloadingProcessChanged>
    {
        public static readonly DownloadingProcessChanged Empty = new DownloadingProcessChanged();

        public int Percentage { get; set; }
        public long CurrentKBytes { get; set; }
        public long TotalKBytes { get; set; }
        public string ProviderName { get; set; }
        public bool IsCompleted { get; set; }
        public int BeatmapId { get; set; }
        public bool SearchingBeatmap { get; set; }
        public bool Idle { get; set; }
    }
}
