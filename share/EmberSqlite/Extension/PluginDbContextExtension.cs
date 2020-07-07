using Autofac;
using EmberKernel.Plugins.Components;
using EmberSqlite.Integration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public static class PluginDbContextExtension
    {
        public static void ConfigureDbContext<T>(this IComponentBuilder builder)
            where T : EmberDbContext
        {
            if (!builder.ParentScope.TryResolve<SqliteConfiguration>(out _))
            {
                throw new NullReferenceException("Can't resolve SqliteInformation, try build kernel with 'UseEFSqlite()' option.");
            }
            builder.ConfigureComponent<T>().SingleInstance();
        }

        public static async ValueTask MigrateDbContext<T>(this ILifetimeScope scope, CancellationToken cancellationToken = default)
            where T : EmberDbContext
        {
            var logger = scope.Resolve<ILogger<T>>();
            var conf = scope.Resolve<SqliteConfiguration>();
            if (!scope.TryResolve<T>(out var sqliteDbContext))
            {
                throw new NullReferenceException($"Can't resolve {typeof(T).Name}, ensure configured when 'BuildComponent'");
            }
            try
            {
                var pendingMigrations = await sqliteDbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Count() > 0 && sqliteDbContext.Database.GetDbConnection() is SqliteConnection sqliteConn)
                {
                    logger.LogInformation($"Will apply migrations, backup database...");
                    var backupPath = Path.Combine(Path.GetDirectoryName(conf.DbPath), "backup");
                    var newFile = Path.Combine(backupPath, $"{Path.GetRandomFileName()}.sqlite");
                    if (!Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);

                    var oldConn = new SqliteConnection(new SqliteConnectionStringBuilder()
                    {
                        Cache = SqliteCacheMode.Shared,
                        DataSource = conf.DbPath,
                        Mode = SqliteOpenMode.ReadWriteCreate,
                        
                    }.ToString());
                    var newConn = new SqliteConnection(new SqliteConnectionStringBuilder()
                    {
                        Cache = SqliteCacheMode.Shared,
                        DataSource = newFile,
                        ForeignKeys = true,
                        Mode = SqliteOpenMode.ReadWriteCreate,
                    }.ToString());
                    await oldConn.OpenAsync();
                    oldConn.BackupDatabase(newConn);
                    logger.LogInformation($"Database backup to {newFile}");


                    logger.LogInformation($"Migrating database with context {typeof(T).Name}");
                    var retry = Policy.Handle<Exception>()
                        .WaitAndRetryAsync(new TimeSpan[]
                        {
                    TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(8)
                        });
                    await retry.ExecuteAsync(() => sqliteDbContext.Database.MigrateAsync(cancellationToken));
                    logger.LogInformation($"Migrated database associated with context {typeof(T).Name}");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while migrating the database used on {typeof(T).Name}");
            }
        }
    }
}
