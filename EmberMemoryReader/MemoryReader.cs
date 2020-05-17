using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
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
        private readonly CancellationTokenSource listenTokenSource = new CancellationTokenSource();
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<MemoryDataCollector>().SingleInstance();
            builder.UseConfigurationModel<PorcessListenerConfiguration>();
            builder.UseProcessListener(listen => listen
                .UseLifetimeTracker<OsuProcessTracker, OsuProcessTerminatedEvent>()
                .UsePredictor<OsuProcessPredicator, OsuProcessMatchedEvent>()
                );
        }

        public override Task Initialize(ILifetimeScope scope)
        {
            // handle the event
            scope.Subscription<OsuProcessMatchedEvent, MemoryDataCollector>();
            scope.Subscription<OsuProcessTerminatedEvent, MemoryDataCollector>();
            // search osu! process
            var listener = scope.Resolve<IProcessListener>();
            Task.Run(() => listener.SearchProcessAsync(listenTokenSource.Token));
            return Task.CompletedTask;
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<OsuProcessMatchedEvent, MemoryDataCollector>();
            scope.Unsubscription<OsuProcessTerminatedEvent, MemoryDataCollector>();
            listenTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
