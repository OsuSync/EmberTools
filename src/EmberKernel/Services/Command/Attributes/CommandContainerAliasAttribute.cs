using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CommandContainerAliasAttribute : Attribute
    {
        public string Alias { get; }
        public CommandContainerAliasAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
