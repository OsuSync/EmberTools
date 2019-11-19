using EmberCore.KernelServices.Command.Components;
using EmberCore.KernelServices.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command
{
    public interface ICommandServices
    {
        void Reigster(ICommandComponent commandHandler);
        void Unregister(ICommandComponent commandHandler);
    }
}
