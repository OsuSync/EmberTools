using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberStatisticDatabase.Database;
using Statistic.Abstract.Events;
using Statistic.Exporter.Service;
using System.Threading.Tasks;

namespace Statistic.Exporter
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Statistic Exporter", Version = "1.0")]
    public class StatisticExportPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureDbContext<StatisticContext>();
            builder.ConfigureComponent<StatisticManager>().SingleInstance().PropertiesAutowired();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.MigrateDbContext<StatisticContext>();
            await scope.Resolve<StatisticManager>().InitializeRegisteredFormat();
            scope.Subscription<FormatCreatedEvent, StatisticManager>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.Resolve<StatisticManager>().UninitializeRegisteredFormat();
            scope.Unsubscription<FormatCreatedEvent, StatisticManager>();
        }
    }
}
