using EmberCore.KernelServices.UI.ViewModel.Configuration.Component;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberKernel.Services.UI.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EmberWpfCore.Components.Configuration.View.Component
{
    public class ComponentFactory : DataTemplateSelector
    {
        private static readonly Dictionary<Type, Type> TypeBinding = new Dictionary<Type, Type>();
        static ComponentFactory()
        {
            TypeBinding.Add(typeof(int), typeof(IntegerComponent));
            TypeBinding.Add(typeof(double), typeof(DoubleComponent));
            TypeBinding.Add(typeof(bool), typeof(CheckComponent));
        }
        public static Type GetControl(Type type)
        {
            if (TypeBinding.ContainsKey(type))
            {
                return TypeBinding[type];
            }
            return typeof(StringComponent);
        }
    }
}
