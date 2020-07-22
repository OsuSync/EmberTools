using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension;
using EmberMemoryReader.Abstract.Data;
using ExamplePlugin.Components;
using ExamplePlugin.EventHandlers;
using ExamplePlugin.Models;
using ExamplePlugin.Models.EventSubscription;
using System.Threading.Tasks;

namespace ExamplePlugin
{

    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin
    {
        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.UseCommandContainer<MyCommandComponent>();
            scope.Subscription<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
            scope.Subscription<GameStatusInfo, MemoryReaderHandler>();
            scope.Subscription<BeatmapInfo, MemoryReaderHandler>();
            scope.Subscription<PlayingInfo, MemoryReaderHandler>();
            scope.Subscription<MultiplayerBeatmapIdInfo, MemoryReaderHandler>();
            scope.Subscription<GameModeInfo, MemoryReaderHandler>();
            scope.RegisterUIModel<MyPlugin, MyPluginConfiguration>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.RemoveCommandContainer<MyCommandComponent>();
            scope.Unsubscription<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
            scope.Unsubscription<GameStatusInfo, MemoryReaderHandler>();
            scope.Unsubscription<BeatmapInfo, MemoryReaderHandler>();
            scope.Unsubscription<PlayingInfo, MemoryReaderHandler>();
            scope.Unsubscription<MultiplayerBeatmapIdInfo, MemoryReaderHandler>();
            scope.UnregisterUIModel<MyPlugin, MyPluginConfiguration>();
            return default;
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.UsePluginOptionsModel<MyPlugin, MyPluginConfiguration>();
            builder.ConfigureUIModel<MyPlugin, MyPluginConfiguration>();
            builder.ConfigureCommandContainer<MyCommandComponent>();
            builder.ConfigureStaticEventHandler<MemoryReaderHandler>();
            builder.ConfigureEventHandler<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
        }

    }
}
