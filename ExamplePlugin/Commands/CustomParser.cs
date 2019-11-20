using EmberCore.KernelServices.Command.Models;
using EmberCore.KernelServices.Command.Parsers;
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
