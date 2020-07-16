using EmberKernel.Services.EventBus;

namespace BeatmapDownloader.Abstract.Models.Events
{
    public class BeatmapDownloadTaskStarted : Event<BeatmapDownloadTaskStarted>
    {
        public DownloadBeatmapSetTask Task { get; set; }
    }
}
