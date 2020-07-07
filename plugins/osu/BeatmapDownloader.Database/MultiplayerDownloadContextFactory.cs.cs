using BeatmapDownloader.Database.Database;
using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

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
