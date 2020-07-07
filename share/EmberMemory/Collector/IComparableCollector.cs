using EmberKernel.Services.EventBus;
using System;

namespace EmberMemory.Components.Collector
{
    public interface IComparableCollector<T> : ICollector<T>
        where T : Event<T>, IComparable<T>, IEquatable<T>
    {
    }
}
