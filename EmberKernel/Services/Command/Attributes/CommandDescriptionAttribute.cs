using System;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        public string Description { get; }
        public CommandDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}
