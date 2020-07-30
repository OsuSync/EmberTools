using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic.Extension;
using EmberMemoryReader.Abstract.Data;
using System.Threading.Tasks;

namespace EmberMemoryReader
{
    [EmberPlugin(Author = "Deliay", Name = "Ember Memory Reader Statistic", Version = "0.0.1")]
    public class MemoryReaderStatistic : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureEventStatistic<BeatmapInfo>();
            builder.ConfigureEventStatistic<GameModeInfo>();
            builder.ConfigureEventStatistic<GameStatusInfo>();
            builder.ConfigureEventStatistic<GlobalGameModeratorInfo>();
            builder.ConfigureEventStatistic<MultiplayerBeatmapIdInfo>();
            builder.ConfigureEventStatistic<PlayingInfo>();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Track<BeatmapInfo>();
            scope.Track<GameModeInfo>();
            scope.Track<GameStatusInfo>();
            scope.Track<GlobalGameModeratorInfo>();
            scope.Track<MultiplayerBeatmapIdInfo>();
            scope.Track<PlayingInfo>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Untrack<BeatmapInfo>();
            scope.Untrack<GameModeInfo>();
            scope.Untrack<GameStatusInfo>();
            scope.Untrack<GlobalGameModeratorInfo>();
            scope.Untrack<MultiplayerBeatmapIdInfo>();
            scope.Untrack<PlayingInfo>();
            return default;
        }
    }
}
