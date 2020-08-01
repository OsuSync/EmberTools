using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using System.Collections.Concurrent;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;

namespace Statistic.Outputs
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Statistic Exporter - MemoryMappedFile", Version = "1.0")]
    public class MemoryMappingFileOutput : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder) { }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            var hub = scope.Resolve<IStatisticHub>();
            hub.OnFormatUpdated += Hub_OnFormatUpdated;
            return default;
        }

        private readonly ConcurrentDictionary<string, MemoryMappedFile> OutputCache = new ConcurrentDictionary<string, MemoryMappedFile>();
        private void Hub_OnFormatUpdated(string name, string format, string value)
        {
            var inst = MemoryMappedFile.CreateOrOpen(name, 2048);
            if (!OutputCache.ContainsKey(name) && !OutputCache.TryAdd(name, inst))
            {
                inst.Dispose();
            }
            Write(OutputCache[name], value);
        }

        private void Write(MemoryMappedFile mmf, string value)
        {
            using var stream = mmf.CreateViewStream();
            var valueBytes = Encoding.UTF8.GetBytes(value);
            stream.Write(valueBytes);
            stream.WriteByte(0);
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            var hub = scope.Resolve<IStatisticHub>();
            hub.OnFormatUpdated -= Hub_OnFormatUpdated;
            foreach (var (_, mmf) in OutputCache)
            {
                Write(mmf, string.Empty);
                mmf.Dispose();
            }
            OutputCache.Clear();
            return default;
        }
    }
}
