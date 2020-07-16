using EmberKernel.Services.EventBus;

namespace BeatmapDownloader.Abstract.Models.Events
{
    public class BeatmapDownloadTaskCompleted : Event<BeatmapDownloadTaskCompleted>
    {
        public DownloadBeatmapSetTask Task { get; set; }
    }
}
