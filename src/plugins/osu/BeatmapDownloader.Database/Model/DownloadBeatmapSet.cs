using System;

namespace BeatmapDownloader.Database.Model
{
    public class DownloadBeatmapSet
    {
        public int Id { get; set; }
        public int BeatmapId { get; set; }
        public int BeatmapSetId { get; set; }
        public string DownloadProviderId { get; set; }
        public string DownloadProviderName { get; set; }
        public string FullPath { get; set; }
        public DateTime DownloadTime { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public DownloadStatus Status { get; set; }
    }
}
