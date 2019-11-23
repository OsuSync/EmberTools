using EmberWeb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Models
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
