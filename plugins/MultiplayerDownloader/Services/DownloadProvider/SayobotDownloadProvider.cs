using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services.DownloadProvider
{
#pragma warning disable IDE1006 // Naming Styles
    class SayobotBeatmapSet
    {
        public int sid { get; set; }
    }
    class SayobotApiResult
    {
        public SayobotBeatmapSet data { get; set; }
        public int status { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles

    public class SayobotDownloadProvider : IDownloadProvier, IComponent
    {
        private HttpClient Requestor { get; }
        private WebClient Downloader { get; }
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public event Action<int, long, long> DownloadProgressChanged;

        public SayobotDownloadProvider()
        {

            Requestor = new HttpClient();
            var downloaderAssembly = typeof(MultiplayerDownloader).Assembly.GetName(false);
            var emberKernelVersion = typeof(IComponent).Assembly.GetName(false);
            var executeVersion = Assembly.GetEntryAssembly().GetName();
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(downloaderAssembly.Name, downloaderAssembly.Version.ToString()));
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(emberKernelVersion.Name, emberKernelVersion.Version.ToString()));
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(executeVersion.Name, executeVersion.Version.ToString()));


            Downloader = new WebClient();
            Downloader.Headers.Add(HttpRequestHeader.UserAgent, Requestor.DefaultRequestHeaders.UserAgent.ToString());
            Downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
        }

        private void Downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged?.Invoke(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
        }

        public async ValueTask Download(string targetPath, string url)
        {
            await Downloader.DownloadFileTaskAsync(url, targetPath);
        }

        public ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true)
        {
            var downloadType = noVideo ? "novideo" : "full";
            return new ValueTask<string>($"https://txy1.sayobot.cn/beatmaps/download/{downloadType}/{beatmapSetId}");
        }

        public async ValueTask<int> GetSetId(int beatmapId)
        {
            var request = (await Requestor
                .GetAsync($"https://api.sayobot.cn/v2/beatmapinfo?K={beatmapId}&T=1", tokenSource.Token))
                .EnsureSuccessStatusCode();
            using var stream = await request.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<SayobotApiResult>(stream, cancellationToken: tokenSource.Token);
            return result.data.sid;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
