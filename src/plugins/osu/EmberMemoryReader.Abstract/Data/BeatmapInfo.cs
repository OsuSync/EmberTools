using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmberMemoryReader.Abstract.Data
{
    [EventNamespace("MemoryReader")]
    public class BeatmapInfo : Event<BeatmapInfo>, IComparable<BeatmapInfo>, IEquatable<BeatmapInfo>
    {
        public bool HasValue { get; set; }
        [DataSourceVariable]
        public int BeatmapId { get; set; }
        [DataSourceVariable]
        public int SetId { get; set; }
        [DataSourceVariable]
        public string BeatmapFile { get; set; }
        [DataSourceVariable]
        public string BeatmapFolder { get; set; }

        public int CompareTo([AllowNull] BeatmapInfo other)
        {
            return this.BeatmapId - other.BeatmapId;
        }

        public bool Equals([AllowNull] BeatmapInfo other)
        {
            return this.BeatmapId == other.BeatmapId;
        }
    }
}
