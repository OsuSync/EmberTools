using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;

namespace BeatmapDownloader.Abstract.Services.DownloadProvider
{
    public class HttpUtils : IDisposable
    {
        public HttpClient Requestor { get; }
        public WebClient Downloader { get; }
        public HttpUtils()
        {
            Requestor = new HttpClient();
            var downloaderAssembly = typeof(HttpUtils).Assembly.GetName(false);
            var emberKernelVersion = typeof(IComponent).Assembly.GetName(false);
            var executeVersion = Assembly.GetEntryAssembly().GetName();
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(downloaderAssembly.Name, downloaderAssembly.Version.ToString()));
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(emberKernelVersion.Name, emberKernelVersion.Version.ToString()));
            Requestor.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(executeVersion.Name, executeVersion.Version.ToString()));


            Downloader = new WebClient();
            Downloader.Headers.Add(HttpRequestHeader.UserAgent, Requestor.DefaultRequestHeaders.UserAgent.ToString());
            Downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
        }

        public event Action<int, long, long> DownloadProgressChanged;
        private void Downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged?.Invoke(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
        }

        public bool TryGetFileNameFromDownloader(out string fileName)
        {
            if (!string.IsNullOrEmpty(Downloader.ResponseHeaders["Content-Disposition"]))
            {
                string header_contentDisposition = Downloader.ResponseHeaders["content-disposition"];
                var disposition = new ContentDisposition(header_contentDisposition);
                if (disposition.FileName != null && disposition.FileName.Length > 0)
                {
                    fileName = HttpUtility.UrlDecode(disposition.FileName);
                    return true;
                }
            }
            fileName = null;
            return false;
        }

        public void Dispose()
        {
            Requestor.Dispose();
            Downloader.Dispose();
        }
    }
}
