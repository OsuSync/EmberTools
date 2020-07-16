using System.Text.Json;

namespace BeatmapDownloader.Extension
{
    public static class JsonSerializerExtension
    {
        public static TDest Cast<TDest>(this object src)
            where TDest : new()
        {
            return JsonSerializer.Deserialize<TDest>(JsonSerializer.Serialize(src));
        }
    }
}
