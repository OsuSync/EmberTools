using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using LyricsFinder;
using LyricsFinder.SourcePrivoder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberLyricDisplayerPlugin.Kernel
{
    public class LyricsFinder : IComponent
    {
        private readonly IPluginOptions<EmberLyricDisplayerPlugin, PluginOptions> optionFactory;
        private readonly SourceProviderBase lyricsProvider;

        private static bool _providersLoad = false;

        public LyricsFinder(IPluginOptions<EmberLyricDisplayerPlugin, PluginOptions> optionFactory)
        {
            this.optionFactory = optionFactory;
            var opt = this.optionFactory.Create();

            if (!_providersLoad)
            {
                InitProvider();
                _providersLoad = true;
            }

            lyricsProvider = SourceProviderManager.GetOrCreateSourceProvier(opt.LyricsSource);
        }

        private void InitProvider()
        {
            //load default impls
            SourceProviderManager.LoadDefaultProviders();

            //todo: load others provider
        }

        public Task<Lyrics> GetLyrics(string title, string artist, int time, CancellationToken cancellationToken = default)
        {
            if (lyricsProvider == null)
                return null;

            return lyricsProvider.ProvideLyricAsync(artist, title, time, false, cancellationToken);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
