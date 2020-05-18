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
    public class MemoryDataCollector : IComponent,
        IEventHandler<OsuProcessMatchedEvent>,
        IEventHandler<OsuProcessTerminatedEvent>
    {
        private ILifetimeScope CurrentScope { get; set; }
        private ILifetimeScope ManagerScope { get; set; }
        public MemoryDataCollector(ILifetimeScope scope)
        {
            this.CurrentScope = scope;
        }

        CancellationTokenSource tokenSource;
        Task IEventHandler<OsuProcessMatchedEvent>.Handle(OsuProcessMatchedEvent @event)
        {
            tokenSource = new CancellationTokenSource();
            return StartCollectorAsync(@event);
        }

        Task IEventHandler<OsuProcessTerminatedEvent>.Handle(OsuProcessTerminatedEvent @event)
        {
            using (tokenSource) tokenSource.Cancel();
            return Task.CompletedTask;
        }

        public async Task StartCollectorAsync(OsuProcessMatchedEvent @event)
        {
            var process = Process.GetProcessById(@event.ProcessId);
            if (process == null) return;

            this.ManagerScope = CurrentScope.BeginLifetimeScope((builder) =>
            {
                builder
                .ReadMemoryWith<WindowsReader>()
                .UseOsuProcessEvent(@event)
                .UseCollectorManager(manager => manager
                    .Collect<Beatmap>()
                    .Collect<GameStatus>()
                    .Collect<Playing>()
                );
            });
            var manager = ManagerScope.Resolve<ICollectorManager>();
            await manager.StartCollectors(tokenSource.Token);
        }

        public void Dispose() 
        {
            tokenSource.Cancel();
        }
    }
}
