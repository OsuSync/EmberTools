using EmberWeb.Data;
using EmberWeb.Model;
using EmberWeb.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmberWeb.Services
{
    public class PluginContextService : IPluginContextService
    {
        private readonly EmberWebContext _emberWebContext;

        public PluginContextService(EmberWebContext emberWebContext)
        {
            _emberWebContext = emberWebContext;
        }

        public async Task<List<Plugin>> SearchPlugins(string name = null, string author = null, string description = null, int start = 0, int limit = 10)
        {
            IQueryable<Plugin> query = _emberWebContext.Plugins.Where(plugin => !plugin.Deleted);
            if (name != null) query.Where(plugin => plugin.Name == name);
            if (author != null) query.Where(plugin => plugin.Author == author);
            if (description != null) query.Where(plugin => plugin.Description == description);

            return await query.Take(limit).Skip(start).ToListAsync().Async();
        }

        public async Task<Plugin> GetPluginById(int id, string version = null)
        {
            var plugin = await _emberWebContext.Plugins
                .Where(plugin => !plugin.Deleted)
                .SingleOrDefaultAsync(plugin => plugin.Id == id);

            await _emberWebContext.Entry(plugin)
                .Collection(p => p.PluginVersions)
                .Query()
                .Where(ver => ver.Version.Contains(version))
                .LoadAsync().Async();

            return plugin;
        }

        public Task<Plugin> CreatePlugin(Plugin plugin, PluginVersion version)
        {
            return _emberWebContext.Database.AutoCommitTransactionScope(async () =>
            {
                if (version.DownloadUrl != null) plugin.DownloadUrl = version.DownloadUrl;
                version.Plugin = plugin;

                // validate create result
                await _emberWebContext.AddAsync(plugin).Async();
                await _emberWebContext.SaveChangeAndValid(count: 1).Async();

                // create a default version for create plugin
                await _emberWebContext.AddAsync(version).Async();
                await _emberWebContext.SaveChangeAndValid(count: 1).Async();

                await _emberWebContext.Entry(plugin)
                .Collection(p => p.PluginVersions)
                .LoadAsync().Async();

                return plugin;
            }).Async();
        }

        public Task<Plugin> UpdatePlugin(Plugin plugin)
        {
            if (!_emberWebContext.Entry(plugin).IsKeySet)
            {
                throw new Exception("Plugin not created");
            }
            return _emberWebContext.Database.AutoCommitTransactionScope(async () =>
            {
                _emberWebContext.Update(plugin);
                await _emberWebContext.SaveChangeAndValid(count: 1).Async();
                return plugin;
            }).Async();
        }

        public Task<Plugin> DeltePlugin(Plugin plugin)
        {
            if (plugin.Id == 0)
            {
                throw new Exception("Plugin not created");
            }
            return _emberWebContext.Database.AutoCommitTransactionScope(async () =>
            {
                _emberWebContext.Remove(plugin);
                await _emberWebContext.SaveChangeAndValid(count: 1);
                return plugin;
            }).Async();
        }

        public Task<List<Plugin>> DeletePluginRange(List<Plugin> plugins)
        {
            if (plugins.All(plugin => _emberWebContext.Entry(plugin).IsKeySet))
            {
                throw new Exception("Invalid plugin create status");
            }

            return _emberWebContext.Database.AutoCommitTransactionScope(async () =>
            {
                _emberWebContext.RemoveRange(plugins);
                await _emberWebContext.SaveChangeAndValid(count: plugins.Count);
                return plugins;
            }).Async();
        }

        public Task<PluginVersion> CreateVersion(PluginVersion version)
        {
            var verEntry = _emberWebContext.Entry(version);
            if (!verEntry.IsKeySet
                || !verEntry.Reference(ver => ver.Plugin).EntityEntry.IsKeySet)
            {
                throw new Exception("Invalid model status");
            }

            return _emberWebContext.Database.AutoCommitTransactionScope(async () =>
            {
                await _emberWebContext.AddAsync(version).Async();
                await _emberWebContext.SaveChangeAndValid(count: 1);

                return version;
            }).Async();
        }
    }
}
