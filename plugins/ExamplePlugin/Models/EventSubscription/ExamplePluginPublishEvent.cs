using EmberKernel.Services.EventBus;

namespace ExamplePlugin.Models.EventSubscription
{
    public class ExamplePluginPublishEvent : Event<ExamplePluginPublishEvent>
    {
        public int InputNumber { get; set; }
    }
}
