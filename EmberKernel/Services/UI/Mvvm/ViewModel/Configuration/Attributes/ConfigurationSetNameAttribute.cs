using System;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Attributes
{
    public class ConfigurationSetNameAttribute : Attribute
    {
        public string Name { get; }
        public ConfigurationSetNameAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
