using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EmberLyricDisplayerPlugin.Models
{
    public class PlayingInfo : Event<PlayingInfo>, IEquatable<PlayingInfo>
    {
        public int PlayingTime { get; set; }

        public bool Equals([AllowNull] PlayingInfo other) => PlayingTime == other.PlayingTime;
    }
}
