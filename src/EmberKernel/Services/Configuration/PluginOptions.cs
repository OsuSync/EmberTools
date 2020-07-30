using EmberKernel.Plugins;
using Microsoft.Extensions.Options;

namespace EmberKernel.Services.Configuration
{
    public class PluginOptions<TPlugin, TOptions> : OptionWriter<TOptions>, IPluginOptions<TPlugin, TOptions>
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        private IOptionsFactory<TOptions> Factory { get; }
        public PluginOptions(IOptionsFactory<TOptions> options, PluginOptionsSetting setting)
            : base(setting, Namespace)
        {
            this.Factory = options;
        }

        private static string Namespace => $"{typeof(TPlugin).Name}.{typeof(TOptions).Name}";

        public TOptions Create()
        {
            return Factory.Create($"{typeof(TPlugin).Name}.{typeof(TOptions).Name}");
        }
    }
}
