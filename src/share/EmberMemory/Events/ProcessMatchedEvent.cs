using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Components
{
    public class ProcessMatchedEvent<T> : Event<T> where T : ProcessMatchedEvent<T>
    {
        public int ProcessId { get; set; }
    }
}
