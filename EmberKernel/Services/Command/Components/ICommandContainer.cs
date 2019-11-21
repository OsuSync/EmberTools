using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Components
{
    public interface ICommandContainer
    {
        virtual void GeneralCommandHandler(string cmd, string[] args) { }
    }
}
