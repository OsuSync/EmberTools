using EmberWeb.Data;
using EmberWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Services
{
    public class PluginsService : IPluginsService
    {
        private EmberWebContext _emberWebContext;

        public PluginsService(EmberWebContext emberWebContext)
        {
            _emberWebContext = emberWebContext;
        }

        public async Task<List<Plugin>> SearchPlugins(string name = null, string author = null, string description = null, int start = 0, int limit = 10)
        {
            IQueryable<Plugin> query = _emberWebContext.Plugins.Where(plugin => !plugin.Deleted);
            if (name != null) query.Where(plugin => plugin.Name == name);
            if (author != null) query.Where(plugin => plugin.Author == author);
            if (description != null) query.Where(plugin => plugin.Description == description);

            return await query.Take(limit).Skip(start).ToListAsync();
        }

        public async Task<Plugin> GetPluginById(int id, string version = null)
        {
            var plugin = await _emberWebContext.Plugins
                .Where(plugin => !plugin.Deleted)
                .SingleOrDefaultAsync(plugin => plugin.Id == id);
            var pluginVerQuery = _emberWebContext.PluginVersions.Where(plugin => plugin.Plugin.Id == plugin.Id);
            if (version != null) pluginVerQuery.Where(ver => ver.Version.Contains(version));
            plugin.PluginVersions = await pluginVerQuery.ToListAsync();
            return plugin;
        }

        public Task<bool> CreatePlugin(EmberUser user, Plugin plugin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePlugin(Plugin plugin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeltePlugin(Plugin plugin)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateVersion(Plugin plugin, PluginVersion version)
        {
            throw new NotImplementedException();
        }
    }
}
