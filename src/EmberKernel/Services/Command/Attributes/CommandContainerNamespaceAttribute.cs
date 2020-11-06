using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =  false, Inherited = false)]
    public class CommandContainerNamespaceAttribute : Attribute
    {
        public string Namespace { get; }
        public CommandContainerNamespaceAttribute(string @namespace)
        {
            this.Namespace = @namespace;
        }
    }
}
