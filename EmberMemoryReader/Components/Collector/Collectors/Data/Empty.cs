using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public class EmptyInfo : Event<EmptyInfo>, IComparable<EmptyInfo>, IEquatable<EmptyInfo>
    {
        public string Scarlet { get; set; }
        public DateTimeOffset Time { get; set; }

        public int CompareTo([AllowNull] EmptyInfo other)
        {
            return (int)(this.Time - other.Time).Ticks;
        }

        public bool Equals([AllowNull] EmptyInfo other)
        {
            return this.Time == other.Time;
        }
    }

    public class Empty : IComparableCollector<EmptyInfo>
    {
        public int ReadInterval { get; set; } = 1000;
        public int RetryLimit { get; set; } = 2;

        public bool TryInitialize()
        {
            return true;
        }

        public bool TryRead(out Event result)
        {
            result = new EmptyInfo()
            {
                Scarlet = "Empty Read Result",
                Time = DateTimeOffset.Now,
            };
            return true;
        }
    }
}
