using BeatmapDownloader.Database.Database;
using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore.Design;

namespace BeatmapDownloader.Database
{
    public class BeatmapDownloaderContextFactory : IDesignTimeDbContextFactory<BeatmapDownloaderDatabaseContext>
    {
        public BeatmapDownloaderDatabaseContext CreateDbContext(string[] args)
        {
            return new BeatmapDownloaderDatabaseContext(new SqliteConfiguration("migrate.db"));
        }
    }
}
