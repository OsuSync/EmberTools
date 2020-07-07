using EmberKernel.Services.EventBus;

namespace EmberKernel.Plugins.Models
{
    public class EmberInitializedEvent : Event<EmberInitializedEvent>
    {
        public static EmberInitializedEvent Empty = new EmberInitializedEvent();
    }
}
