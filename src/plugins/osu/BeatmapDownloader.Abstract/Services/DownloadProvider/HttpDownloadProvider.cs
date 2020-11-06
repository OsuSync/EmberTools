using Autofac;
using BeatmapDownloader.Abstract.Services.UI;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeatmapDownloader.Abstract.Services.DownloadProvider
{
    public abstract class HttpDownloadProvider : IDownloadProvier
    {
        public event Action<int, long, long> DownloadProgressChanged;
        protected CancellationTokenSource CancellationSource { get; }
        protected HttpUtils HttpUtils { get; }
        public HttpDownloadProvider()
        {
            CancellationSource = new CancellationTokenSource();
            HttpUtils = new HttpUtils();
            HttpUtils.DownloadProgressChanged += Downloader_DownloadProgressChanged;
        }
        private void Downloader_DownloadProgressChanged(int percent, long bytesReceived, long totalBytesToReceive)
        {
            DownloadProgressChanged?.Invoke(percent, bytesReceived, totalBytesToReceive);
        }

        public void Dispose()
        {
            CancellationSource.Cancel();
            CancellationSource.Dispose();
            HttpUtils.Dispose();
        }
        public async ValueTask<string> Download(string targetFile, string url)
        {
            await HttpUtils.Downloader.DownloadFileTaskAsync(url, targetFile);
            HttpUtils.TryGetFileNameFromDownloader(out var fileName);
            return fileName;
        }
        public abstract ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true);
        public abstract ValueTask<int?> GetSetId(int beatmapId);

        public virtual ValueTask Initialize(ILifetimeScope scope) => default;

        public virtual ValueTask Uninitialize(ILifetimeScope scope) => default;

        public override string ToString()
        {
            return this.GetProviderListDisplayName();
        }

    }
}
