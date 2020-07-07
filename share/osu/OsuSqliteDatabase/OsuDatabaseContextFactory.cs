using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore.Design;
using OsuSqliteDatabase.Database;

namespace OsuSqliteDatabase
{
    public class OsuDatabaseContextFactory : IDesignTimeDbContextFactory<OsuDatabaseContext>
    {
        public OsuDatabaseContext CreateDbContext(string[] args)
        {
            return new OsuDatabaseContext(new SqliteConfiguration("migrate.db"));
        }

    }
}
