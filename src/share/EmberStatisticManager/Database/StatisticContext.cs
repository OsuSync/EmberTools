using EmberSqlite.Integration;
using EmberStatisticDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace EmberStatisticDatabase.Database
{
    public class StatisticContext : EmberDbContext
    {
        public DbSet<RegisteredFormat> RegisteredFormats { get; set; }
        public StatisticContext(SqliteConfiguration configuration) : base(configuration) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisteredFormat>(entity =>
            {
                entity.HasIndex(entity => entity.Name).IsUnique();
            });
        }
    }
}
