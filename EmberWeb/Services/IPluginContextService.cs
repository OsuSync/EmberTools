using EmberWeb.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmberWeb.Services
{
    public interface IPluginContextService
    {
        Task<Plugin> CreatePlugin(Plugin plugin, PluginVersion version);
        Task<Plugin> UpdatePlugin(Plugin plugin);
        Task<Plugin> DeltePlugin(Plugin plugin);
        Task<PluginVersion> CreateVersion(PluginVersion version);
        Task<Plugin> GetPluginById(int id, string version = null);
        Task<List<Plugin>> SearchPlugins(string name = null, string author = null, string description = null, int start = 0, int limit = 10);
    }
}
