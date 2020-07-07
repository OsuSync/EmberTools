using EmberKernel.Services.EventBus;

namespace ExamplePlugin.Models.EventSubscription
{
    [EventNamespace("MemoryReader")]
    public class BeatmapInfo : Event<BeatmapInfo>
    {
        public bool HasValue { get; set; }
        public int BeatmapId { get; set; }
        public int SetId { get; set; }
        public string BeatmapFile { get; set; }
        public string BeatmapFolder { get; set; }
    }
}
