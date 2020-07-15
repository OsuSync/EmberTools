using System;

namespace BeatmapDownloader.Abstract.Models
{
    public class DownloadBeatmapSetTask
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public string DownloadUrl { get; set; }
        public int BeatmapSetId { get; set; }
        public int BeatmapId { get; set; }
        public string DownloadProviderId { get; set; }
        public string DownloadProviderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

    }
}
