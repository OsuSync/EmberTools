using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPluginsLoader : IScopeBuilder
    {
        ValueTask RunEntryComponents();
    }
}
