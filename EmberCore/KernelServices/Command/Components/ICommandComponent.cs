using EmberCore.KernelServices.Command.Parsers;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Components
{
    public interface ICommandComponent : IComponent
    {
        virtual void GeneralCommandHandler(string cmd, string[] args) { }
    }
}
