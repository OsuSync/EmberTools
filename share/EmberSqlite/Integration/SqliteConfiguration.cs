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
