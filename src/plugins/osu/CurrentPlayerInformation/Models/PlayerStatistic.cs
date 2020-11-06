namespace CurrentPlayerInformation.Models
{
    public class PlayerStatistic
    {
        public PlayerLevel Level { get; set; }
        public double PP { get; set; }
        public int PPRank{ get; set; }
        public long RankedScore { get; set; }
        public double HitAccuracy { get; set; }
        public int PlayCount { get; set; }
        public int PlayTime { get; set; }
        public long TotalScore { get; set; }
        public int TotalHit { get; set; }
        public int MaximumCombo { get; set; }
        public int ReaplysWatchedByOthers { get; set; }
        public bool IsRanked { get; set; }
        public PlayerGradeCount GradeCounts { get; set; }
    }
}
