using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmberCore.KernelServices.UI.View
{
    public static class PluginComponentBuilderExtension
    {
        private static IWindowManager<Window> Resolve(ILifetimeScope scope)
        {
            if (scope.TryResolve<IWindowManager<Window>>(out var instance))
            {
                return instance;
            }
            throw new NullReferenceException();
        }
        public static void ConfigureWpfWindow<T>(this IComponentBuilder builder) where T : Window, IHostedWindow, new()
        {
            Resolve(builder.ParentScope).Register<T>();
        }

        public static ValueTask InitializeWpfWindow<T>(this ILifetimeScope scope) where T : Window, IHostedWindow, new()
        {
            return Resolve(scope).InitializeAsync<T>(scope);
        }

        public static ValueTask UninitializeWpfWindow<T>(this ILifetimeScope scope) where T : Window, IHostedWindow, new()
        {
            return Resolve(scope).UninitializeAsync<T>(scope);
        }
    }
}
