using System;
using System.Collections.Generic;
using System.Text;

namespace EmberSqlite.Integration
{
    public class SqliteConfiguration
    {
        public string DbPath { get; }
        public SqliteConfiguration(string dbPath)
        {
            DbPath = dbPath;
        }
    }
}
