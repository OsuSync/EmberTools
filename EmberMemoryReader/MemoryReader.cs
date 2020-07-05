using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemory.Components.Collector;
using EmberMemory.Listener;
using EmberMemoryReader.Components;
using EmberMemoryReader.Components.Osu;
using EmberMemoryReader.Components.Osu.Listener;
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
            builder.UseMemoryDataCollector<OsuMemoryDataCollector, OsuProcessMatchedEvent, OsuProcessTerminatedEvent>();
            builder.UsePluginOptionsModel<MemoryReader, PorcessListenerConfiguration>();
            builder.UseProcessListener(listen => listen
                .UseLifetimeTracker<OsuProcessTracker, OsuProcessTerminatedEvent>()
                .UsePredictor<OsuProcessPredicator, OsuProcessMatchedEvent>()
                );
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            // handle the event
            scope.Subscription<EmberInitializedEvent, IProcessListener>();
            scope.Subscription<OsuProcessMatchedEvent, OsuMemoryDataCollector.MatchedHandler>();
            scope.Subscription<OsuProcessTerminatedEvent, OsuMemoryDataCollector.TerminatedHandler>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<EmberInitializedEvent, IProcessListener>();
            scope.Unsubscription<OsuProcessMatchedEvent, OsuMemoryDataCollector.MatchedHandler>();
            scope.Unsubscription<OsuProcessTerminatedEvent, OsuMemoryDataCollector.TerminatedHandler>();
            return default;
        }
    }
}
