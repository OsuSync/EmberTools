using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Listener
{
    public class OsuProcessTracker : IProcessLifetimeTracker<OsuProcessTerminatedEvent>
    {
        public ProcessLifeTimeReport<OsuProcessTerminatedEvent> Report(Process process)
        {
            return new ProcessLifeTimeReport<OsuProcessTerminatedEvent>()
            {
                Terminated = process.HasExited,
                Report = !process.HasExited ? default : new OsuProcessTerminatedEvent() {  ProcessId = process.Id }
            };
        }
    }
}
