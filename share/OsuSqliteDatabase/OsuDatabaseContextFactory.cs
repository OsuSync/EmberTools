using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OsuSqliteDatabase.Database;
using System;
using System.Collections.Generic;
using System.Text;

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
