using EmberCore.KernelServices.UI.View.Configuration.Components;
using System;
using System.Collections.Generic;

namespace EmberCore.KernelServices.UI.View.Configuration
{
    public class ComponentFactory
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
