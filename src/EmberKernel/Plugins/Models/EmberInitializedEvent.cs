using EmberKernel.Services.EventBus;

namespace EmberKernel.Plugins.Models
{
    public class EmberInitializedEvent : Event<EmberInitializedEvent>
    {
        public static readonly EmberInitializedEvent Empty = new EmberInitializedEvent();
    }
}
