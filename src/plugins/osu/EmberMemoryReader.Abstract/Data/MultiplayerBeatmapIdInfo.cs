using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmberMemoryReader.Abstract.Data
{
    [EventNamespace("MemoryReader")]
    public class MultiplayerBeatmapIdInfo : Event<MultiplayerBeatmapIdInfo>, IComparable<MultiplayerBeatmapIdInfo>, IEquatable<MultiplayerBeatmapIdInfo>
    {
        public bool HasValue { get; set; }

        [DataSourceVariable(Name = "MultiplayerBeatmapId")]
        public int BeatmapId { get; set; }

        public int CompareTo([AllowNull] MultiplayerBeatmapIdInfo other)
        {
            return other.BeatmapId - BeatmapId;
        }

        public bool Equals([AllowNull] MultiplayerBeatmapIdInfo other)
        {
            return other.BeatmapId == BeatmapId;
        }
    }
}
