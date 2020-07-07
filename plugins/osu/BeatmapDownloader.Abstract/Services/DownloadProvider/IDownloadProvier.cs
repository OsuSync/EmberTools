using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Threading.Tasks;

namespace BeatmapDownloader.Abstract.Services.DownloadProvider
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
