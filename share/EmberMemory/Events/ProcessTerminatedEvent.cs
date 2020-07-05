using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Components
{
    public class ProcessTerminatedEvent<T> : Event<T> where T : ProcessTerminatedEvent<T>
    {
        public int ProcessId { get; set; }
    }
}
