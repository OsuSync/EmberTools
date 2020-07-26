using EmberKernel.Services.EventBus;
using System;

namespace EmberMemoryReader.Abstract.Data
{
    public class GlobalGameModeratorInfo : Event<GlobalGameModeratorInfo>, IComparable<GlobalGameModeratorInfo>, IEquatable<GlobalGameModeratorInfo>
    {
        public int GlobalRawModerator { get; set; }
        public int CompareTo(GlobalGameModeratorInfo other)
        {
            return GlobalRawModerator - other.GlobalRawModerator;
        }

        public bool Equals(GlobalGameModeratorInfo other)
        {
            return GlobalRawModerator == other.GlobalRawModerator;
        }
    }
}
