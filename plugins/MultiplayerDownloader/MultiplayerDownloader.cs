using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using MultiplayerDownloader.Models;
using MultiplayerDownloader.Services;
using System;
using System.Threading.Tasks;

namespace MultiplayerDownloader
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Multiplayer Beatmap Auto Downloader", Version = "1.0")]
    public class MultiplayerDownloader : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<BeatmapDownloadService>().SingleInstance();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Subscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<MultiplayerBeatmapIdInfo, BeatmapDownloadService>();
            return default;
        }
    }
}
