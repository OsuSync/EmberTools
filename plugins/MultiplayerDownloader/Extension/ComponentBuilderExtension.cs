using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using MultiplayerDownloader.Services.DownloadProvider;
using MultiplayerDownloader.Services.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerDownloader.Extension
{
    public static class ComponentBuilderExtension
    {
        private static IViewComponentManager Resolve(ILifetimeScope scope)
        {
            if (!scope.TryResolve<IViewComponentManager>(out var manager))
            {
                throw new NullReferenceException("Kernel MVVM service not initliazed, call 'UseMvvmInterface' when building kernel");
            }
            return manager;
        }
        public static void ConfigureDownloadProvider<T>(this IComponentBuilder builder)
            where T : IDownloadProvier, IComponent, new()
        {
            Resolve(builder.ParentScope).RegisterComponent<T>();
            builder.ConfigureComponent<T>().AsSelf().Named<IDownloadProvier>(typeof(T).Name);
        }

    }
}
