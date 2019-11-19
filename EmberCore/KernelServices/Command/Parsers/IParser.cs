using EmberCore.KernelServices.Command.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Parsers
{
    public interface IParser
    {
        IEnumerable<object> ParseCommandArgument(CommandArgument args);

    }
}
