using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Components.Collector
{
    public interface ICollector
    {
        public int ReadInterval { get; set; }
        public int RetryLimit { get; set; }
        public bool TryInitialize();
        public bool TryRead(out Event result);
    }
    public interface ICollector<T> : ICollector
        where T : Event
    {
    }
}
