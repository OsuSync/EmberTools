using System;
using System.Collections.Generic;
using System.Text;

namespace OsuSqliteDatabase.Model
{
    public class OsuDatabaseBeatmapStarRating
    {
        public int Id { get; set; }
        public int OsuDatabaseBeatmapId { get; set; }
        public OsuDatabaseBeatmap OsuDatabaseBeatmap { get; set; }
        public OsuGameRuleSet RuleSet { get; set; }
        public OsuGameModerator Moderators { get; set; }
        public double StarRating { get; set; }

    }
}
