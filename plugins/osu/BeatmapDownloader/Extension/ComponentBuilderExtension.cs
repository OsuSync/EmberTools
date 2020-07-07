using Autofac;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;

namespace BeatmapDownloader.Extension
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
