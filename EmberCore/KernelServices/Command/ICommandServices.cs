using EmberCore.KernelServices.Command.Builder;
using EmberCore.KernelServices.Command.Components;
using EmberCore.KernelServices.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberCore.KernelServices.Command
{
    public interface ICommandServices : IDisposable
    {
        void ReigsterCommandContainer(ICommandContainer commandHandler);
        void RemoveHandler(ICommandContainer commandHandler);

        Task ConfigureCommandSource(Action<ICommandSourceBuilder> builder);
    }
}
