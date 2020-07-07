using EmberCore.KernelServices.UI.View.Configuration;
using EmberCore.KernelServices.UI.ViewModel.Configuration;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationItemViewModel
    {
        public ConfigurationSingleValueViewModel Value { get; }
        public string Name { get; }
        public Type ValueType { get; }
        public Control InputControl { get; set; }

        public ConfigurationItemViewModel(Type propertyType, string name, DependencySet dependencySet, IWindowManager windowManager)
        {
            Name = name;
            ValueType = propertyType;
            var type = ComponentFactory.GetControl(propertyType);
            if (dependencySet.HasAttach(WpfFeatureBuilder.WpfRenderControl))
            {
                var customControls = dependencySet.GetAttach<Dictionary<string, Type>>(WpfFeatureBuilder.WpfRenderControl);
                if (customControls.ContainsKey(name))
                {
                    type = customControls[name];
                }
            }
            if (dependencySet.HasAttach(WpfFeatureBuilder.WpfMultiOptionFields))
            {
                var multiSet = dependencySet.GetAttach<HashSet<string>>(WpfFeatureBuilder.WpfMultiOptionFields);
                if (multiSet.Contains(name))
                {
                    Value = new ConfigurationMultiValueViewModel(name, dependencySet);
                }
            }
            if (Value == null)
            {
                Value = new ConfigurationSingleValueViewModel(name, dependencySet);
            }

            windowManager.BeginUIThreadScope(() =>
            {
                InputControl = (Control)Activator.CreateInstance(type);
                InputControl.DataContext = Value;
            });
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
