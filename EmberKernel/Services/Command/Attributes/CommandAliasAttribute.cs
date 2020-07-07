using System;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CommandAliasAttribute : Attribute
    {
        public string Alias { get; }
        public CommandAliasAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
