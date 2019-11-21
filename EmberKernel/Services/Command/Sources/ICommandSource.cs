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
        Task Initialize(CancellationToken cancellationToken);
        Task Stop(CancellationToken cancellationToken);
        Task<CommandArgument> Read(CancellationToken cancellationToken);
    }
}
