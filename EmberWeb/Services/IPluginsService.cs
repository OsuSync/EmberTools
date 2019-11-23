using System.Collections.Generic;
using System.Threading.Tasks;
using EmberWeb.Data;

namespace EmberWeb.Services
{
    public interface IPluginsService
    {
        Task<bool> CreatePlugin(EmberUser user, Plugin plugin);
        Task<bool> UpdatePlugin(Plugin plugin);
        Task<bool> DeltePlugin(Plugin plugin);
        Task<bool> CreateVersion(Plugin plugin, PluginVersion version);
        Task<Plugin> GetPluginById(int id, string version = null);
        Task<List<Plugin>> SearchPlugins(string name = null, string author = null, string description = null, int start = 0, int limit = 10);
    }
}