using System;
using System.Collections.Generic;

namespace CurrentPlayerInformation.Models
{
    public class PlayerInformation
    {
        public string AvatarUrl { get; set; }
        public string CoverUrl { get; set; }
        public string CountryCode { get; set; }
        public bool IsOnline { get; set; }
        public DateTimeOffset JoinDate { get; set; }
        public string Username { get; set; }
        public List<PlayerMonthlyPlayCount> MonthlyPlaycounts { get; set; }
        public PlayerStatistic Statistics { get; set; }
    }
}
