using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using MultiplayerDownloader.Services.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services.DownloadProvider
{
    [ViewComponentNamespace("MultiplayerDownloader.IDownloadProvier")]
    public interface IDownloadProvier : IComponent, IViewComponent
    {
        ValueTask<int?> GetSetId(int beatmapId);
        ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true);
        ValueTask<string> Download(string targetFile, string url);

        event Action<int, long, long> DownloadProgressChanged;

    }
}
