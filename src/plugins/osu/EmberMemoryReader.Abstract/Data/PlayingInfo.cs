using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EmberMemoryReader.Abstract.Data
{
    [EventNamespace("MemoryReader")]
    public class PlayingInfo : Event<PlayingInfo>, IComparable<PlayingInfo>, IEquatable<PlayingInfo>
    {
        public bool HasValue { get; set; }
        [DataSourceVariable]
        public int RawModInfo { get; set; }
        [DataSourceVariable]
        public int RawListeningModInfo { get; set; }
        [DataSourceVariable]
        public string CurrentPlayerName { get; set; }
        [DataSourceVariable]
        public int PlayingTime { get; set; }
        public List<int> RawUnstableRate { get; set; }
        [DataSourceVariable]
        public double Accuracy { get; set; }
        [DataSourceVariable]
        public double HP { get; set; }
        /// <summary>
        /// Current score
        /// </summary>
        [DataSourceVariable]
        public int Score { get; set; }
        /// <summary>
        /// Current combo
        /// </summary>
        [DataSourceVariable]
        public int Combo { get; set; }
        /// <summary>
        /// miss
        /// </summary>
        [DataSourceVariable]
        public int Missing { get; set; }
        /// <summary>
        /// 300
        /// </summary>
        [DataSourceVariable]
        public int Best { get; set; }
        /// <summary>
        /// 100
        /// </summary>
        [DataSourceVariable]
        public int Good { get; set; }
        /// <summary>
        /// 50
        /// </summary>
        [DataSourceVariable]
        public int Bad { get; set; }
        /// <summary>
        /// geki(osu!)/ 300g(mania)
        /// </summary>
        [DataSourceVariable]
        public int Geki { get; set; }
        /// <summary>
        /// katu(osu!)/ 200(mania)
        /// </summary>
        [DataSourceVariable]
        public int Katu { get; set; }

        public int CompareTo([AllowNull] PlayingInfo other)
        {
            return -1;
        }

        public bool Equals([AllowNull] PlayingInfo other)
        {
            return (!this.HasValue && !other.HasValue)
                || (this.HasValue == other.HasValue
                && this.Score == other.Score
                && this.PlayingTime == other.PlayingTime
                && this.RawModInfo == other.RawModInfo);
        }
    }
}
