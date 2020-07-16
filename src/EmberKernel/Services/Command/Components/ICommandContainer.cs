using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Components
{
    public interface ICommandContainer
    {
        bool TryAssignCommand(CommandArgument argument, out CommandArgument newArgument);
    }
}
