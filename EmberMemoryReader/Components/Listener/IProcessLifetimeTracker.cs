using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Listener
{
    public interface IProcessLifetimeTracker<T> where T : Event<T>
    {
        ProcessLifeTimeReport<T> Report(Process process);
    }
}
