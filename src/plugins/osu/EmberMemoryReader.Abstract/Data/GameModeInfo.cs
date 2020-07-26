using EmberKernel.Services.EventBus;
using System;

namespace EmberMemoryReader.Abstract.Data
{
    public enum OsuMode
    {
        Osu = 0,
        Taiko = 1,
        CatchTheBeat = 2,
        Mania = 3,
        Unknown = -1
    }

    [EventNamespace("MemoryReader")]
    public class GameModeInfo : Event<GameModeInfo>, IComparable<GameModeInfo>, IEquatable<GameModeInfo>
    {
        public bool HasValue { get; set; }
        public OsuMode Mode { get; set; }
        public int CompareTo(GameModeInfo other)
        {
            return (int)Mode - (int)other.Mode;
        }

        public bool Equals(GameModeInfo other)
        {
            return Mode == other.Mode;
        }
    }
}
