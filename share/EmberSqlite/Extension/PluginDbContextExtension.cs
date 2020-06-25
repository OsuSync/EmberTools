using Autofac;
using EmberKernel.Plugins.Components;
using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public static class PluginDbContextExtension
    {
        public static void ConfigureDbContext<T>(this IComponentBuilder builder)
            where T : EmberDbContext
        {
            if (!builder.ParentScope.TryResolve<SqliteConfiguration>(out var sqliteConf))
            {
                throw new NullReferenceException("Can't resolve SqliteInformation, try build kernel with 'UseEFSqlite()' option.");
            }
            builder.ConfigureComponent<T>().SingleInstance();
        }

        public static async ValueTask SqliteApplyMigration<T>(this ILifetimeScope scope, CancellationToken cancellationToken = default)
            where T : EmberDbContext
        {
            if (!scope.TryResolve<T>(out var sqliteDbContext))
            {
                throw new NullReferenceException($"Can't resolve {typeof(T).Name}, ensure configured when 'BuildComponent'");
            }
            await sqliteDbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
