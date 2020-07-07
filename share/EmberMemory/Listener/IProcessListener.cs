﻿using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus.Handlers;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Listener
{
    public interface IProcessListener : IComponent, IEventHandler<EmberInitializedEvent>
    {
        ValueTask SearchProcessAsync(CancellationToken token);
        ValueTask EnsureProcessLifetime(Process process, CancellationToken token);
    }
}
