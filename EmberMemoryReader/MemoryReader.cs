using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Components;
using EmberMemoryReader.Components.Collector;
using EmberMemoryReader.Components.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader
{
    [EmberPlugin(Author = "KedamaOvO, ZeroAsh", Name = "EmberMemoryReader", Version = "0.1")]
    public class MemoryReader : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<MemoryDataCollector>().SingleInstance();
            builder.UsePluginOptionsModel<MemoryReader, PorcessListenerConfiguration>();
            builder.UseProcessListener(listen => listen
                .UseLifetimeTracker<OsuProcessTracker, OsuProcessTerminatedEvent>()
                .UsePredictor<OsuProcessPredicator, OsuProcessMatchedEvent>()
                );
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            // handle the event
            scope.Subscription<EmberInitializedEvent, MemoryDataCollector>();
            scope.Subscription<OsuProcessMatchedEvent, MemoryDataCollector>();
            scope.Subscription<OsuProcessTerminatedEvent, MemoryDataCollector>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<EmberInitializedEvent, MemoryDataCollector>();
            scope.Unsubscription<OsuProcessMatchedEvent, MemoryDataCollector>();
            scope.Unsubscription<OsuProcessTerminatedEvent, MemoryDataCollector>();
            return default;
        }
    }
}
