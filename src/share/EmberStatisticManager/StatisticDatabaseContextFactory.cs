using EmberSqlite.Integration;
using EmberStatisticDatabase.Database;
using Microsoft.EntityFrameworkCore.Design;

namespace EmberStatisticDatabase
{
    public class StatisticContextFactory : IDesignTimeDbContextFactory<StatisticContext>
    {
        public StatisticContext CreateDbContext(string[] args)
        {
            return new StatisticContext(new SqliteConfiguration("migrate.db"));
        }

    }
}
