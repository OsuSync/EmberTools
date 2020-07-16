using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.HelpGenerator
{
    public interface ICommandHelp
    {
        public Type CommandContainerType { get; }
        public IDictionary<string, CommandInfo> CommandInformation { get; }

        public void HandleHelp(CommandArgument argument);
    }
}
