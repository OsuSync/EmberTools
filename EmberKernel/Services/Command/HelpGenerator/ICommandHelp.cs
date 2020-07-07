using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;

namespace EmberKernel.Services.Command.HelpGenerator
{
    public interface ICommandHelp
    {
        public Type CommandContainerType { get; }
        public IDictionary<string, CommandInfo> CommandInformation { get; }

        public void HandleHelp(CommandArgument argument);
    }
}
