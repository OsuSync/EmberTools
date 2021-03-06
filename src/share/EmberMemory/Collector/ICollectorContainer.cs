﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Components.Collector
{
    public interface ICollectorContainer
    {
        public ValueTask Run(CancellationToken cancellationToken);
    }
    public interface ICollectorContainer<T> : ICollectorContainer where T : ICollector
    {
    }
}
