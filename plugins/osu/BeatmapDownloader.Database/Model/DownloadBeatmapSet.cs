using System;
using System.Collections.Generic;
using System.Text;

namespace BeatmapDownloader.Database.Model
{
    public class DownloadBeatmapSet
    {
        public int Id { get; set; }
        public int BeatmapId { get; set; }
        public int BeatmapSetId { get; set; }
        public string DownloadProviderId { get; set; }
        public string DownloadProviderName { get; set; }
        public DateTime DownloadTime { get; set; }
    }
}
