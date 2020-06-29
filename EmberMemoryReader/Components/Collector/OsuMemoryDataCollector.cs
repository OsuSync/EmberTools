using Autofac;
using EmberMemoryReader.Components.Collector.Collectors;
using EmberMemoryReader.Components.Collector.Collectors.Data;
using EmberMemoryReader.Components.Collector.Readers;
using EmberMemoryReader.Components.Collector.Readers.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector
{
    public class OsuMemoryDataCollector : MemoryDataCollector<OsuMemoryDataCollector, OsuProcessMatchedEvent, OsuProcessTerminatedEvent>
    {
        public OsuMemoryDataCollector(ILifetimeScope scope) : base(scope)
        {
        }

        protected override bool BuildCollectScope(CollectorBuilder builder, OsuProcessMatchedEvent @event)
        => builder
            .ReadMemoryWith<WindowsReader>()
            .UseOsuProcessEvent(@event)
            .UseCollectorManager(manager => manager
                .Collect<Beatmap>()
                .Collect<GameStatus>()
                .Collect<Playing>()
                .Collect<MultiplayerBeatmapId>()
            )
            .Build();
    }
}
