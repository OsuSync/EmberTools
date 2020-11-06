using EmberMemory.Listener;
using EmberMemoryReader.Abstract.Events;
using System.Diagnostics;

namespace EmberMemoryReader.Components.Osu.Listener
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
