using EmberKernel.Services.EventBus;

namespace EmberMemory.Components
{
    public class ProcessTerminatedEvent<T> : Event<T> where T : ProcessTerminatedEvent<T>
    {
        public int ProcessId { get; set; }
    }
}
