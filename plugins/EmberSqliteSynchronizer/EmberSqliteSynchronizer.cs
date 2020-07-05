using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberSqliteSynchronizer.Component;
using EmberSqliteSynchronizer.Models;
using OsuSqliteDatabase.Database;
using System;
using System.Threading.Tasks;

namespace EmberSqliteSynchronizer
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Ember Sqlite Synchronizer", Version = "1.0")]
    public class EmberSqliteSynchronizer : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureDbContext<OsuDatabaseContext>();
            builder.ConfigureComponent<DatabaseSynchronizer>().SingleInstance();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.MigrateDbContext<OsuDatabaseContext>();
            scope.Subscription<OsuProcessMatchedEvent, DatabaseSynchronizer>();
            scope.Subscription<OsuProcessTerminatedEvent, DatabaseSynchronizer>();
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<OsuProcessMatchedEvent, DatabaseSynchronizer>();
            scope.Unsubscription<OsuProcessTerminatedEvent, DatabaseSynchronizer>();
            return default;
        }
    }
}
