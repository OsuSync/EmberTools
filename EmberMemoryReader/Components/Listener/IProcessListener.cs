using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Listener
{
    public interface IProcessListener : IComponent
    {
        ValueTask SearchProcessAsync(CancellationToken token);
        ValueTask EnsureProcessLifetime(Process process, CancellationToken token);
    }
}
