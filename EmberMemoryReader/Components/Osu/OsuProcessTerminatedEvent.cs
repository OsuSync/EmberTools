using EmberKernel.Services.EventBus;
using EmberMemory.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Osu
{
    public class OsuProcessTerminatedEvent : ProcessTerminatedEvent<OsuProcessTerminatedEvent>
    {
    }
}
