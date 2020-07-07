using EmberKernel.Services.Command.Builder;
using EmberKernel.Services.Command.Components;
using System;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command
{
    public interface ICommandService : IDisposable, IAsyncDisposable
    {
        void RegisterCommandContainer(ICommandContainer commandHandler, bool enableCommandHelp = true);
        void UnregisterCommandContainer(ICommandContainer commandHandler);

        ValueTask ConfigureCommandSource(Action<ICommandSourceBuilder> builder);
    }
}
