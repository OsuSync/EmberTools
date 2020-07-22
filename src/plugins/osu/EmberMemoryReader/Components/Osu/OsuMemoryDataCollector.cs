using Autofac;
using EmberMemory.Components.Collector;
using EmberMemory.Readers;
using EmberMemory.Readers.Windows;
using EmberMemoryReader.Abstract.Data;
using EmberMemoryReader.Abstract.Events;

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
                .Collect<GameMode>()
                .Collect<GameStatus>()
                .Collect<Playing>()
                .Collect<MultiplayerBeatmapId>()
            )
            .Build();
    }
}
