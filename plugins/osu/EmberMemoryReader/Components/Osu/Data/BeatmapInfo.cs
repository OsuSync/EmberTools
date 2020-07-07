using EmberKernel.Services.EventBus;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmberMemoryReader.Components.Osu.Data
{
    [EventNamespace("MemoryReader")]
    public class BeatmapInfo : Event<BeatmapInfo>, IComparable<BeatmapInfo>, IEquatable<BeatmapInfo>
    {
        public bool HasValue { get; set; }
        public int BeatmapId { get; set; }
        public int SetId { get; set; }
        public string BeatmapFile { get; set; }
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
