using EmberKernel.Services.EventBus;

namespace EmberMemory.Components
{
    public class ProcessMatchedEvent<T> : Event<T> where T : ProcessMatchedEvent<T>
    {
        public int ProcessId { get; set; }
    }
}
