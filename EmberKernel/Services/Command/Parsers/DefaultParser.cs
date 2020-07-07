using EmberKernel.Services.Command.Models;
using System.Collections.Generic;

namespace EmberKernel.Services.Command.Parsers
{
    public class DefaultParser : IParser
    {
        public IEnumerable<object> ParseCommandArgument(CommandArgument argument)
        {
            return new[] { argument.Argument };
        }
    }
}
