using Autofac;
using EmberMemory.Components.Collector;
using EmberMemory.Readers;
using EmberMemory.Readers.Windows;
using EmberMemoryReader.Components.Osu.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Osu
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
