using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command.Sources
{
    public interface ICommandSource : IDisposable
    {
        ValueTask Initialize(CancellationToken cancellationToken);
        ValueTask Stop(CancellationToken cancellationToken);
        ValueTask<CommandArgument> Read(CancellationToken cancellationToken);
    }
}
