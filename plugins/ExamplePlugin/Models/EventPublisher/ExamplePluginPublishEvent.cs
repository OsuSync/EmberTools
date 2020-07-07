using EmberKernel.Services.EventBus;

namespace ExamplePlugin.Models.EventPublisher
{
    public class ExamplePluginPublishEvent : Event<ExamplePluginPublishEvent>
    {
        public int InputNumber { get; set; }
    }
}
