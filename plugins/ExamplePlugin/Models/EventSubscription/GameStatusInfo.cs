using EmberKernel.Services.EventBus;

namespace ExamplePlugin.Models.EventSubscription
{
    [EventNamespace("MemoryReader")]
    public class GameStatusInfo : Event<GameStatusInfo>
    {
        public bool HasValue { get; set; }
        public int Status { get; set; }
        public string StringStatus { get; set; }
    }
}
