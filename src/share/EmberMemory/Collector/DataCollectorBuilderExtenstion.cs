using EmberKernel;
using EmberKernel.Plugins.Components;
using EmberMemory.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Components.Collector
{
    public static class DataCollectorBuilderExtenstion
    {
        public static void UseMemoryDataCollector<TCollector, TMatched, TTerminated>(this IComponentBuilder builder)
            where TCollector : MemoryDataCollector<TCollector, TMatched, TTerminated>
            where TMatched : ProcessMatchedEvent<TMatched>
            where TTerminated : ProcessTerminatedEvent<TTerminated>
        {
            builder.ConfigureComponent<TCollector>().SingleInstance();
            builder.ConfigureStaticEventHandler<MemoryDataCollector<TCollector, TMatched, TTerminated>.MatchedHandler>();
            builder.ConfigureStaticEventHandler<MemoryDataCollector<TCollector, TMatched, TTerminated>.TerminatedHandler>();
        }
    }
}
