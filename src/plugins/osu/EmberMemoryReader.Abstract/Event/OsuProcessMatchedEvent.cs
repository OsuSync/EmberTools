using EmberMemory.Components;

namespace EmberMemoryReader.Abstract.Events
{
    public class OsuProcessMatchedEvent : ProcessMatchedEvent<OsuProcessMatchedEvent>
    {
        public string BeatmapDirectory { get; set; }
        public string LatestVersion { get; set; }
        public string UserName { get; set; }
        public string GameDirectory { get; set; }
    }
}
