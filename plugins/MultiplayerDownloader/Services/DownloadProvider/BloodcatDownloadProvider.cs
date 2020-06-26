using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services.DownloadProvider
{
    public class BloodcatDownloadProvider : IDownloadProvier, IComponent
    {
        public event Action<int, long, long> DownloadProgressChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ValueTask Download(string targetFile, string url)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> GetSetId(int beatmapId)
        {
            throw new NotImplementedException();
        }
    }
}
