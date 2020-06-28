using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Listener
{
    public interface IProcessListener : IComponent, IEventHandler<EmberInitializedEvent>
    {
        ValueTask SearchProcessAsync(CancellationToken token);
        ValueTask EnsureProcessLifetime(Process process, CancellationToken token);
    }
}
