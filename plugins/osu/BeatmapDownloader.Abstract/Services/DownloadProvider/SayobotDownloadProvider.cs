using BeatmapDownloader.Abstract.Services.UI;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeatmapDownloader.Abstract.Services.DownloadProvider
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

    [DownloadProviderList(Name = "Sayobot")]
    [ViewComponentNamespace("MultiplayerDownloader.IDownloadProvier")]
    public class SayobotDownloadProvider : HttpDownloadProvider
    {

        public override ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true)
        {
            var downloadType = noVideo ? "novideo" : "full";
            return new ValueTask<string>($"https://txy1.sayobot.cn/beatmaps/download/{downloadType}/{beatmapSetId}");
        }

        public override async ValueTask<int?> GetSetId(int beatmapId)
        {
            var request = (await HttpUtils.Requestor
                .GetAsync($"https://api.sayobot.cn/v2/beatmapinfo?K={beatmapId}&T=1", CancellationSource.Token))
                .EnsureSuccessStatusCode();
            using var stream = await request.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<SayobotApiResult>(stream, cancellationToken: CancellationSource.Token);
            if (result.status != 0) return null;
            return result.data.sid;
        }
    }
}
