using Autofac;
using CurrentPlayerInformation.Components;
using CurrentPlayerInformation.Models;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic.Extension;
using EmberMemoryReader.Abstract.Data;
using EmberMemoryReader.Abstract.Events;
using System.Threading.Tasks;

namespace CurrentPlayerInformation
{

    [EmberPlugin(Author = "Deliay", Name = "CurrentPlayerInformationPlugin", Version = "0.0.1")]
    public class CurrentPlayerInformationPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureEventStatistic<PlayerInformationEvent>();
            builder.ConfigureComponent<PlayerInformationCrawler>().SingleInstance().PropertiesAutowired();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Subscription<OsuProcessMatchedEvent, PlayerInformationCrawler>();
            scope.Subscription<PlayingInfo, PlayerInformationCrawler>();
            scope.Track<PlayerInformationEvent>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Untrack<PlayerInformationEvent>();
            scope.Unsubscription<OsuProcessMatchedEvent, PlayerInformationCrawler>();
            scope.Unsubscription<PlayingInfo, PlayerInformationCrawler>();
            return default;
        }
    }
}
