using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;

namespace CurrentPlayerInformation.Models
{
    public class PlayerInformationEvent : Event<PlayerInformationEvent>
    {
        [DataSourceVariable]
        public string AvatarUrl { get; set; }

        [DataSourceVariable]
        public string CoverUrl { get; set; }

        [DataSourceVariable]
        public string CountryCode { get; set; }

        [DataSourceVariable]
        public bool IsOnline { get; set; }

        [DataSourceVariable]
        public DateTimeOffset JoinDate { get; set; }

        [DataSourceVariable]
        public string Username { get; set; }

        [DataSourceVariable]
        public int SS { get; set; }

        [DataSourceVariable]
        public int Ssh { get; set; }

        [DataSourceVariable]
        public int S { get; set; }

        [DataSourceVariable]
        public int Sh { get; set; }

        [DataSourceVariable]
        public int A { get; set; }

        [DataSourceVariable]
        public int CurrentLevel { get; set; }

        [DataSourceVariable]
        public int LevelProgress { get; set; }

        [DataSourceVariable]
        public double PP { get; set; }

        [DataSourceVariable]
        public int PPRank { get; set; }

        [DataSourceVariable]
        public int GlobalRank { get; set; }

        [DataSourceVariable]
        public int CountryRank { get; set; }

        [DataSourceVariable]
        public long RankedScore { get; set; }

        [DataSourceVariable]
        public double HitAccuracy { get; set; }

        [DataSourceVariable]
        public int PlayCount { get; set; }

        [DataSourceVariable]
        public int PlayTime { get; set; }

        [DataSourceVariable]
        public long TotalScore { get; set; }

        [DataSourceVariable]
        public int TotalHit { get; set; }

        [DataSourceVariable]
        public int MaximumCombo { get; set; }

        [DataSourceVariable]
        public int ReaplysWatchedByOthers { get; set; }

        [DataSourceVariable]
        public bool IsRanked { get; set; }
        public static PlayerInformationEvent FromPlayerInformation(PlayerInformation information)
        {
            return new PlayerInformationEvent()
            {
                A = information.Statistics.GradeCounts.A,
                SS = information.Statistics.GradeCounts.SS,
                S = information.Statistics.GradeCounts.S,
                Sh = information.Statistics.GradeCounts.Sh,
                Ssh = information.Statistics.GradeCounts.Ssh,
                AvatarUrl = information.AvatarUrl,
                CountryCode = information.CountryCode,
                CoverUrl = information.CoverUrl,
                CurrentLevel = information.Statistics.Level.Current,
                LevelProgress = information.Statistics.Level.Progress,
                PP = information.Statistics.PP,
                PPRank = information.Statistics.GlobalRank,
                GlobalRank = information.Statistics.GlobalRank,
                CountryRank = information.Statistics.CountryRank,
                RankedScore = information.Statistics.RankedScore,
                HitAccuracy = information.Statistics.HitAccuracy,
                PlayCount = information.Statistics.PlayCount,
                PlayTime = information.Statistics.PlayTime,
                TotalScore = information.Statistics.TotalScore,
                TotalHit = information.Statistics.TotalHit,
                MaximumCombo = information.Statistics.MaximumCombo,
                ReaplysWatchedByOthers = information.Statistics.ReaplysWatchedByOthers,
                IsRanked = information.Statistics.IsRanked,
                Username = information.Username,
                IsOnline = information.IsOnline,
                JoinDate = information.JoinDate,
            };
        }
    }
}
