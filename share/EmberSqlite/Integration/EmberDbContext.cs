using EmberKernel.Plugins.Components;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberSqlite.Integration
{
    public abstract class EmberDbContext : DbContext, IComponent
    {
        protected SqliteConfiguration SqliteInformation { get; }
        
        public EmberDbContext(SqliteConfiguration configuration)
        {
            this.SqliteInformation = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder()
            {
                Cache = SqliteCacheMode.Shared,
                DataSource = SqliteInformation.DbPath,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWriteCreate,
                RecursiveTriggers = true,
            }.ToString());
        }
    }
}
