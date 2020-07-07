using System;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CommandHandlerAttribute: Attribute
    {
        public string Command { get; set; }
    }
}
