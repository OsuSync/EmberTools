using EmberKernel.Services.EventBus;

namespace BeatmapDownloader.Abstract.Models.Events
{
    public class BeatmapDownloadAddressPrepared : Event<BeatmapDownloadAddressPrepared>
    {
        public string DownloadUrl { get; set; }
        public int BeatmapSetId { get; set; }
        public int BeatmapId { get; set; }
    }
}
