using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using MultiplayerDownloader.Services.DownloadProvider;
using MultiplayerDownloader.Services.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Extension
{
    public static class DownloadProviderUIExtension
    {
        private static IViewComponentManager Resolve(ILifetimeScope scope)
        {
            if (!scope.TryResolve<IViewComponentManager>(out var manager))
            {
                throw new NullReferenceException("Kernel MVVM service not initliazed, call 'UseMvvmInterface' when building kernel");
            }
            return manager;
        }
        public static ValueTask AddDownloadProviderUIOptions<T>(this ILifetimeScope scope)
            where T : IDownloadProvier, new()
        {
            return Resolve(scope).InitializeComponent(scope, typeof(T));
        }

        public static ValueTask RemoveDownloadProviderUIOptions<T>(this ILifetimeScope scope)
            where T : IDownloadProvier
        {
            return Resolve(scope).UninitializeComponent(scope, typeof(T));
        }
    }
}
