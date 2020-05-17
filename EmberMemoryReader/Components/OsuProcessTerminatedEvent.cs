using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components
{
    public class OsuProcessTerminatedEvent : Event<OsuProcessTerminatedEvent>
    {
        public int ProcessId { get; set; }
    }
}
