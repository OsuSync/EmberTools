using EmberWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace EmberWeb.Data
{
    public class EmberWebContext : DbContext
    {
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<PluginVersion> PluginVersions { get; set; }

        public EmberWebContext(DbContextOptions<EmberWebContext> options)
        : base(options)
        { }
    }
}
