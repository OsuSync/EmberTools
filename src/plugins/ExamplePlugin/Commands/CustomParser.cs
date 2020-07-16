using EmberKernel.Services.Command.Models;
using EmberKernel.Services.Command.Parsers;
using System.Collections.Generic;

namespace ExamplePlugin.Commands
{
    public class CustomParser : IParser
    {
        public IEnumerable<object> ParseCommandArgument(CommandArgument args)
        {
            if (int.TryParse(args.Argument, out var parsedInt)) yield return parsedInt;
            else yield return 0;
        }
    }
}
