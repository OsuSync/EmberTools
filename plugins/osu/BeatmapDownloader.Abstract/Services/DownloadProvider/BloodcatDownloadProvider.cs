using BeatmapDownloader.Abstract.Services.UI;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeatmapDownloader.Abstract.Services.DownloadProvider
{

#pragma warning disable IDE1006 // Naming Styles
    class BloodcatBeatmap
    {
        public string id { get; set; }
    }
    class BloodcatBeatmapSet
    {
        public string id { get; set; }
        public List<BloodcatBeatmap> beatmaps { get; set; }

    }
#pragma warning restore IDE1006 // Naming Styles

    [DownloadProviderList(Name = "Bloodcat")]
    [ViewComponentNamespace("MultiplayerDownloader.IDownloadProvier")]
    public class BloodcatDownloadProvider : HttpDownloadProvider
    {

        public override ValueTask<string> GetBeatmapSetDownloadUrl(int beatmapSetId, bool noVideo = true)
        {
            if (noVideo)
            {
                var bloodcatCookie = $"DLOPT={JsonSerializer.Serialize(new { bg = false, video = noVideo, skin = false, cdn = false })}";
                base.HttpUtils.Downloader.Headers.Add(HttpRequestHeader.Cookie, bloodcatCookie);
            }
            return new ValueTask<string>($"https://bloodcat.com/osu/s/{beatmapSetId}");
        }

        public override async ValueTask<int?> GetSetId(int beatmapId)
        {
            var request = (await HttpUtils.Requestor
                .GetAsync($"https://bloodcat.com/osu/?mod=json&c=b&q={beatmapId}", CancellationSource.Token))
                .EnsureSuccessStatusCode();
            using var stream = await request.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<BloodcatBeatmapSet>>(stream, cancellationToken: CancellationSource.Token);
            if (result.Count == 0)
            {
                return null;
            }
            return int.Parse(result[0].id);
        }
    }
}
