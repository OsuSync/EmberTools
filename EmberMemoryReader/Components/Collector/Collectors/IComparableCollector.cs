using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public interface IComparableCollector<T> : ICollector<T>
        where T : Event<T>, IComparable<T>, IEquatable<T>
    {
    }
}
