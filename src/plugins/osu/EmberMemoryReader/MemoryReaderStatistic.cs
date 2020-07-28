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
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Track<BeatmapInfo>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Track<BeatmapInfo>();
            return default;
        }
    }
}
