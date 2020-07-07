using EmberKernel.Services.EventBus;

namespace ExamplePlugin.Models.EventSubscription
{
    [EventNamespace("MemoryReader")]
    public class MultiplayerBeatmapIdInfo : Event<MultiplayerBeatmapIdInfo>
    {
        public bool HasValue { get; set; }
        public int BeatmapId { get; set; }
    }
}
