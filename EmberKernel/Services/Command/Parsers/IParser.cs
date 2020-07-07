using EmberKernel.Services.Command.Models;
using System.Collections.Generic;

namespace EmberKernel.Services.Command.Parsers
{
    public interface IParser
    {
        IEnumerable<object> ParseCommandArgument(CommandArgument args);

    }
}
