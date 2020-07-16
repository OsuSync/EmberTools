using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyPropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
