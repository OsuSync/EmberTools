using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services.DownloadProvider
{
    public interface IDownloadProvier
    {
        ValueTask<int> GetSetId(int beatmapId);
        ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true);
        ValueTask Download(string targetFile, string url);

        event Action<int, long, long> DownloadProgressChanged;
    }
}
