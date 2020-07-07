using EmberKernel.Plugins;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Configuration
{
    public interface IPluginOptions<TPlugin, TOptions>
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        public TOptions Create();
        public ValueTask SaveAsync(TOptions options, CancellationToken cancellationToken = default);
    }
}
