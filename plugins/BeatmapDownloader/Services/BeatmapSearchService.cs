using Autofac;
using BeatmapDownloader.Abstract.Models;
using BeatmapDownloader.Abstract.Services.DownloadProvider;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using System.Threading.Tasks;

namespace BeatmapDownloader.Services
{
    public class BeatmapSearchService : IComponent
    {
        private ILifetimeScope Scope { get; }
        private IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> OptionFactory { get; }
        public BeatmapSearchService(
            ILifetimeScope scope,
            IReadOnlyPluginOptions<BeatmapDownloaderConfiguration> options)
        {
            Scope = scope;
            OptionFactory = options;
        }

        public ValueTask<int?> SetId(int beatmapId)
        {
            var options = OptionFactory.Create();
            var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider.Id);
            return downloadProvider.GetSetId(beatmapId);
        }

        public ValueTask<string> DownloadAddressBySetId(int beatmapSetId)
        {
            var options = OptionFactory.Create();
            var downloadProvider = Scope.ResolveNamed<IDownloadProvier>(options.DownloadProvider.Id);
            return downloadProvider.GetBeatmapSetDownloadUrl(beatmapSetId);
        }

        public void Dispose()
        {

        }
    }
}
