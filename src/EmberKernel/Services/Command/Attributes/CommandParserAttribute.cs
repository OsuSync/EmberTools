using EmberKernel.Services.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandParserAttribute : Attribute
    {
        public Type Parser { get; }
        public CommandParserAttribute(Type parser)
        {
            this.Parser = parser;
        }
    }
}
