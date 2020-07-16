using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Parsers
{
    public interface IParser
    {
        IEnumerable<object> ParseCommandArgument(CommandArgument args);

    }
}
