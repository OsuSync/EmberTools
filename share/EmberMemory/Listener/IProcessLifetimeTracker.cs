using EmberKernel.Services.EventBus;
using System.Diagnostics;

namespace EmberMemory.Listener
{
    public interface IProcessLifetimeTracker<T> where T : Event<T>
    {
        ProcessLifeTimeReport<T> Report(Process process);
    }
}
