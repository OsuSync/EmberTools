using EmberKernel.Services.EventBus;

namespace EmberSqliteSynchronizer.Models
{
    public class OsuProcessMatchedEvent : Event<OsuProcessMatchedEvent>
    {
        public string BeatmapDirectory { get; set; }
        public string LatestVersion { get; set; }
        public string UserName { get; set; }
        public string GameDirectory { get; set; }
        public int ProcessId { get; set; }
    }
}
