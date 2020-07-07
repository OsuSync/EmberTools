using System;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyPropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
