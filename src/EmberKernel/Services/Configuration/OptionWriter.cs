using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Configuration
{
    public class OptionWriter<TOptions>
        where TOptions : class, new()
    {
        private readonly PluginOptionsSetting Setting;
        private readonly string Namespace;
        public OptionWriter(PluginOptionsSetting setting, string @namespace)
        {
            this.Setting = setting;
            this.Namespace = @namespace;
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
