using Autofac;
using EmberKernel.Plugins;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Configuration
{
    public class PluginOptions<TPlugin, TOptions> : IPluginOptions<TPlugin, TOptions>
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        private IOptionsFactory<TOptions> Factory { get; }
        private readonly PluginOptionsSetting Setting;
        public PluginOptions(ILifetimeScope scope, IOptionsFactory<TOptions> options)
        {
            if (!scope.TryResolve(out Setting))
            {
                throw new NullReferenceException("Kernel not configre with .UseOptionsModerator, ensure call UseOptionsModerator when you build your life scope");
            }
            this.Factory = options;
        }

        private string Namespace => $"{typeof(TPlugin).Name}.{typeof(TOptions).Name}";

        public TOptions Create()
        {
            return Factory.Create($"{typeof(TPlugin).Name}.{typeof(TOptions).Name}");
        }

        public async ValueTask SaveAsync(TOptions options, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> dict = default;
            using (var readStream = File.OpenRead(Setting.PersistFilePath))
            {
                dict = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(readStream, cancellationToken: cancellationToken);
                dict.Remove(Namespace);
                dict.Add(Namespace, options);
            }
            if (dict == null)
            {
                throw new NullReferenceException("Read persisted configuration errored!");
            }
            var tempFile = Path.GetTempFileName();
            using (var writeStream = File.OpenWrite(tempFile))
            {
                await JsonSerializer.SerializeAsync(writeStream, dict, cancellationToken: cancellationToken);
            }
            File.Copy(tempFile, Setting.PersistFilePath, overwrite: true);
        }
    }
}
