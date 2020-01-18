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
            builder.ConfigureComponent<MemoryDataCollector>();
            builder.ConfigureComponent<ProcessListener<OsuProcessPredicator, OsuProcessMatchedEvent>>();
        }

        public override async Task Initialize(ILifetimeScope scope)
        {
            // handle the event
            scope.Subscription<OsuProcessMatchedEvent, MemoryDataCollector>();
            // search osu! process
            var listener = scope.Resolve<ProcessListener<OsuProcessPredicator, OsuProcessMatchedEvent>>();
            await listener.SearchProcessAsync(listenTokenSource.Token);
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            listenTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
