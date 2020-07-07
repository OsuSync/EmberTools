using System;
using System.Collections.Generic;

namespace OsuSqliteDatabase.Model
{
    public class OsuDatabase
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int FolderCount { get; set; }
        public bool AccountLocked { get; set; }
        public DateTime UnlockedAt { get; set; }
        public string PlayerName { get; set; }
        public int BeatmapCount { get; set; }
        public List<OsuDatabaseBeatmap> Beatmaps { get; set; }
        public OsuGameBeatmapPermission Permission { get; set; }

    }
}
