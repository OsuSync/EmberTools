using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public class ViewComponentNameAttribute : Attribute
    {
        public string Name { get; }
        public ViewComponentNameAttribute(string name)
        {
            this.Name = name;
        }
    }

    public static class ViewComponentNameAttributeExtension
    {
        public static string GetFriendlyDisplayName(this Type type)
        {
            if (type.GetCustomAttribute<ViewComponentNameAttribute>() is ViewComponentNameAttribute attr)
            {
                return attr.Name;
            }
            return type.Name;
        }
    }
}
