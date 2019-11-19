using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Parsers
{
    public class DefaultParser : IParser
    {
        public IEnumerable<object> ParseCommandArgument(string command, string input)
        {
            throw new NotImplementedException();
        }
    }
}
