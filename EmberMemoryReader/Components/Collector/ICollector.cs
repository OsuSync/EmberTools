using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector
{
    public interface ICollector<T>
    {
        public int ReadInterval { get; set; }
        public int RetryLimit { get; set; }
        public bool TryInitialize();
        public bool TryRead(out T result);
    }
}
