using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OsuSqliteDatabase.Model
{
    public class OsuDatabase
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public bool AccountLocked { get; set; }
        public DateTime UnlockedAt { get; set; }
        public string PlayerName { get; set; }
        public int BeatmapCount { get; set; }
        public int Permission { get; set; }

    }
}
