using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public class Empty : ICollector
    {
        class EmptyInfo : Event<EmptyInfo>
        {
            public string Scarlet { get; set; }
            public DateTimeOffset Time { get; set; }
        }
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
