using EmberWeb.Data;
using EmberWeb.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
