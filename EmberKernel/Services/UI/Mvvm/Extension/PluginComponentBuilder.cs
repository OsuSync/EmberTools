using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Extension
{
    public static class PluginComponentBuilder
    {
        private static IViewComponentManager Resolve(ILifetimeScope scope)
        {
            if (!scope.TryResolve<IViewComponentManager>(out var manager))
            {
                throw new NullReferenceException("Kernel MVVM service not initliazed, call 'UseMvvmInterface' when building kernel");
            }
            return manager;
        }
        public static void ConfigureUIComponent<ICategory, TComponent>(this IComponentBuilder builder) where TComponent : IViewComponent, new()
        {
            Resolve(builder.ParentScope).RegisterComponent<ICategory, TComponent>();
        }
        public static void ConfigureUIComponent<TComponent>(this IComponentBuilder builder) where TComponent : IViewComponent, new()
        {
            Resolve(builder.ParentScope).RegisterComponent<TComponent>();
            builder.ConfigureComponent<TComponent>().SingleInstance();
        }
        public static void ConfigureUIComponent<TComponent>(this IComponentBuilder builder, string category) where TComponent : IViewComponent, new()
        {
            Resolve(builder.ParentScope).RegisterComponent<TComponent>(category);
        }

        public static void InitializeUIComponent<TComponent>(this ILifetimeScope scope) where TComponent : IViewComponent, new()
        {
            Resolve(scope).InitializeComponent(scope, typeof(TComponent));
        }
        public static void UninitializeUIComponent<TComponent>(this ILifetimeScope scope) where TComponent : IViewComponent, new()
        {
            Resolve(scope).UninitializeComponent(scope, typeof(TComponent));
        }
    }
}
