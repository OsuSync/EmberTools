using BeatmapDownloader.Database.Model;
using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore;

namespace BeatmapDownloader.Database.Database
{
    public class BeatmapDownloaderDatabaseContext : EmberDbContext
    {
        public BeatmapDownloaderDatabaseContext(SqliteConfiguration configuration) : base(configuration)
        {
        }

        public DbSet<DownloadBeatmapSet> DownloadedBeatmapSets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DownloadBeatmapSet>().HasIndex((m) => m.DownloadProviderId);
            modelBuilder.Entity<DownloadBeatmapSet>().HasIndex((m) => m.DownloadProviderName);
            base.OnModelCreating(modelBuilder);
        }
    }
}
