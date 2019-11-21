using EmberKernel.Services.Command.Builder;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command
{
    public interface ICommandService : IDisposable
    {
        void ReigsterCommandContainer(ICommandContainer commandHandler);
        void RemoveHandler(ICommandContainer commandHandler);

        Task ConfigureCommandSource(Action<ICommandSourceBuilder> builder);
    }
}
