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
        Task SearchProcessAsync(CancellationToken token);
        Task EnsureProcessLifetime(Process process, CancellationToken token);
    }
}
