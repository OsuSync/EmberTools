using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector
{
    public interface ICollector
    {
        public int ReadInterval { get; set; }
        public int RetryLimit { get; set; }
        public bool TryInitialize();
    }
    public interface ICollector<T> : ICollector
        where T : Event
    {
        public bool TryRead(out T result);
    }
}
