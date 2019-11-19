using EmberCore.KernelServices.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CommandHandlerAttribute: Attribute
    {
        public string Command { get; set; }
        public Type Parser { get; set; } = typeof(DefaultParser);
    }
}
