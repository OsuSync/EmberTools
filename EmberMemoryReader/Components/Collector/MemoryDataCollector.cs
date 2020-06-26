﻿using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Components.Collector.Collectors;
using EmberMemoryReader.Components.Collector.Collectors.Data;
using EmberMemoryReader.Components.Collector.Readers;
using EmberMemoryReader.Components.Collector.Readers.Windows;
using EmberMemoryReader.Components.Listener;
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
        IEventHandler<OsuProcessTerminatedEvent>,
        IEventHandler<EmberInitializedEvent>
    {
        private ILifetimeScope CurrentScope { get; set; }
        private ILifetimeScope ManagerScope { get; set; }
        public MemoryDataCollector(ILifetimeScope scope)
        {
            tokenSource = new CancellationTokenSource();
            this.CurrentScope = scope;
        }

        private readonly CancellationTokenSource tokenSource;
        ValueTask IEventHandler<OsuProcessMatchedEvent>.Handle(OsuProcessMatchedEvent @event)
        {
            return StartCollectorAsync(@event);
        }

        ValueTask IEventHandler<OsuProcessTerminatedEvent>.Handle(OsuProcessTerminatedEvent @event)
        {
            using (tokenSource) tokenSource.Cancel();
            return default;
        }
        public async ValueTask Handle(EmberInitializedEvent @event)
        {
            // search osu! process
            var listener = CurrentScope.Resolve<IProcessListener>();
            await listener.SearchProcessAsync(tokenSource.Token);
        }

        public async ValueTask StartCollectorAsync(OsuProcessMatchedEvent @event)
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
                    .Collect<MultiplayerBeatmapId>()
                );
            });
            var manager = ManagerScope.Resolve<ICollectorManager>();
            await manager.StartCollectors(tokenSource.Token);
        }

        public void Dispose() 
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
    }
}
