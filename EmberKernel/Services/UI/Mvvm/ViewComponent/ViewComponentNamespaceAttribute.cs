using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public class ViewComponentNamespaceAttribute : Attribute
    {
        public string Namespace { get; }
        public ViewComponentNamespaceAttribute(string @namespace)
        {
            this.Namespace = @namespace;
        }
    }

    public static class ViewComponentNamespaceExtension
    {
        public static string GetViewComponentNamespace(this Type type)
        {
            if (type.GetCustomAttribute<ViewComponentNamespaceAttribute>() is ViewComponentNamespaceAttribute attr)
            {
                return $"{attr.Namespace}";
            }
            return $"{type.Name}";
        }

        public static IEnumerable<string> GetAllViewComponentNamespace(this Type type)
        {
            foreach (var attr in type.GetCustomAttributes<ViewComponentNamespaceAttribute>())
            {
                yield return $"{attr.Namespace}";
            }
            yield return $"{type.Name}";
        }

        public static bool IsSameCategoryComponent(this Type type, Type assignTo)
        {
            return type.GetAllViewComponentNamespace().Intersect(assignTo.GetAllViewComponentNamespace()).Any();
        }

        public static bool IsSameCategoryComponent<TAssignTo>(this Type type)
        {
            return IsSameCategoryComponent(type, typeof(TAssignTo));
        }
    }
}
