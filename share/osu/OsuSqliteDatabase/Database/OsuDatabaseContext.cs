using EmberSqlite.Integration;
using Microsoft.EntityFrameworkCore;
using OsuSqliteDatabase.Model;

namespace OsuSqliteDatabase.Database
{
    public class OsuDatabaseContext : EmberDbContext
    {
        public DbSet<OsuDatabase> OsuDatabases { get; set; }
        public DbSet<OsuDatabaseBeatmap> OsuDatabaseBeatmap { get; set; }
        public DbSet<OsuDatabaseBeatmapStarRating> OsuDatabaseBeatmapStarRating { get; set; }
        public DbSet<OsuDatabaseTimings> OsuDatabaseTimings { get; set; }
        public OsuDatabaseContext(SqliteConfiguration config) : base(config) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OsuDatabase>().HasIndex(entity => entity.PlayerName);

            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.BeatmapId);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.BeatmapSetId);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.RankStatus);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.StandardRankRating);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.TaikoRankRating);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.CatchTheBeatRankRating);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.ManiaRankRating);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.NotPlayed);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.Artist);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.ArtistUnicode);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.Title);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.TitleUnicode);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.Creator);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => entity.Difficult);
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => new { entity.FolderName, entity.FileName });
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => new { entity.CircleCount, entity.SliderCount, entity.SpinnerCount });
            modelBuilder.Entity<OsuDatabaseBeatmap>().HasIndex(entity => new { entity.ApproachRate, entity.CircleSize, entity.HPDrain, entity.OverallDifficulty });

            modelBuilder.Entity<OsuDatabaseBeatmapStarRating>().HasIndex(entity => entity.Moderators);
            modelBuilder.Entity<OsuDatabaseBeatmapStarRating>().HasIndex(entity => entity.StarRating);

            modelBuilder.Entity<OsuDatabaseTimings>().HasIndex(entity => entity.BeatPreMinute);
            base.OnModelCreating(modelBuilder);
        }
    }
}
