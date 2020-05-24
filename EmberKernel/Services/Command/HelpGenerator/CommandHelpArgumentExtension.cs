using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Models
{
    public static class CommandHelpArgumentExtension
    {
        public static CommandArgument MoveToHelpCommand(this CommandArgument argument)
        {
            argument.Command = "help";
            return argument;
        }
    }
}
