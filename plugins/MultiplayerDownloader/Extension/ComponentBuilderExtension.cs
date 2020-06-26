using EmberKernel.Plugins.Components;
using MultiplayerDownloader.Services.DownloadProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerDownloader.Extension
{
    public static class ComponentBuilderExtension
    {
        public static void ConfigureDownloadProvider<T>(this IComponentBuilder builder)
            where T : IDownloadProvier, IComponent
        {
            builder.ConfigureComponent<T>().Named<IDownloadProvier>(typeof(T).Name);
        }

    }
}
