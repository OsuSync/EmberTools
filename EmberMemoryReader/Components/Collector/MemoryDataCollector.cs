using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Components.Collector.Collectors;
using EmberMemoryReader.Components.Collector.Collectors.Data;
using EmberMemoryReader.Components.Collector.Readers;
using EmberMemoryReader.Components.Collector.Readers.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Collector
{
    public class MemoryDataCollector : IComponent, IEventHandler<OsuProcessMatchedEvent>
    {
        private IContainer MemoryDataScope { get; }
        private ILifetimeScope CurrentScope { get; set; }
        public MemoryDataCollector(ILifetimeScope scope)
        {
            this.CurrentScope = scope;
        }

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        Task IEventHandler<OsuProcessMatchedEvent>.Handle(OsuProcessMatchedEvent @event)
        {
            return StartCollectorAsync(@event);
        }

        public async Task StartCollectorAsync(OsuProcessMatchedEvent @event)
        {
            var process = Process.GetProcessById(@event.ProcessId);
            if (process == null) return;

            this.CurrentScope = CurrentScope.BeginLifetimeScope((builder) =>
            {
                builder.RegisterInstance(@event);
                builder
                .ReadMemoryWith<WindowsReader>()
                .Collects(builder => builder
                    .Data<Beatmap, BeatmapInfo>()
                    .Data<GameStatus, GameStatusInfo>()
                );
            });
            var manager = this.CurrentScope.Resolve<ICollectorManager>();
            await manager.StartCollectors(tokenSource.Token);
        }

        public void Dispose() 
        {
            tokenSource.Cancel();
        }
    }
}
