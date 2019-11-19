using EmberCore.KernelServices.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Parsers
{
    public class DefaultParser : IParser
    {
        public IEnumerable<object> ParseCommandArgument(CommandArgument argument)
        {
            return new[] { argument.Argument };
        }
    }
}
