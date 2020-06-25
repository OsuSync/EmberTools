using System;
using System.Collections.Generic;
using System.Text;

namespace OsuSqliteDatabase.Model
{
    public class OsuDatabaseBeatmap
    {
        public int Id { get; set; }
        public int OsuDatabaseId { get; set; }
        public OsuDatabase OsuDatabase { get; set; }
        public int BytesOfBeatmapEntry { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Creator { get; set; }
        public string Difficult { get; set; }
        public string AudioFileName { get; set; }
        public string MD5Hash { get; set; }
        public string FileName { get; set; }
        public OsuGameBeatmapRankStatus RankStatus { get; set; }
        public int CircleCount { get; set; }
        public int SliderCount { get; set; }
        public int SpinnerCount { get; set; }
        public DateTime LatestModifiedAt { get; set; }
        public double ApproachRate { get; set; }
        public double CircleSize { get; set; }
        public double HPDrain { get; set; }
        public double OverallDifficulty { get; set; }
        public double SliderVelocity { get; set; }
        public List<OsuDatabaseBeatmapStarRating> StarRatings { get; set; }
        public int TimingPointCount { get; set; }
        public List<OsuDatabaseTimings> OsuDatabaseTimings { get; set; }
        public int BeatmapId { get; set; }
        public int BeatmapSetId { get; set; }
        public int ThreadId { get; set; }
        public OsuGameRankRating StandardRankRating { get; set; }
        public OsuGameRankRating TaikoRankRating { get; set; }
        public OsuGameRankRating CatchTheBeatRankRating { get; set; }
        public OsuGameRankRating ManiaRankRating { get; set; }
        public int LocalOffset { get; set; }
        public double StackLeniency { get; set; }
        public OsuGameRuleSet RuleSet { get; set; }
        public string Score { get; set; }
        public string Tags { get; set; }
        public int OnlineOffset { get; set; }
        public string TitleFont { get; set; }
        public bool NotPlayed { get; set; }
        public bool IsOsz2 { get; set; }
        public string FolderName { get; set; }
        public DateTime LatestUpdatedAt { get; set; }
        public bool BeatmapSoundIgnored { get; set; }
        public bool BeatmapSkinIgnored { get; set; }
        public bool StoryboardDisabled { get; set; }
        public bool VideoDisabled { get; set; }
        public bool VisualOverrided { get; set; }
        public int ManiaScrollSpeed { get; set; }

    }
}
