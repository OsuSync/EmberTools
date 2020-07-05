using Autofac;
using EmberKernel;
using EmberKernel.Plugins.Components;
using EmberMemory.Components;
using EmberMemory.Components.Collector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Osu
{
    public static class OsuDataCollectorBuilderExtenstion
    {
        public static CollectorBuilder UseOsuProcessEvent(this CollectorBuilder builder, OsuProcessMatchedEvent @event)
        {
            builder.Builder.RegisterInstance(@event).SingleInstance();
            builder.Builder.Register((ctx) =>
            {
                var process = Process.GetProcessById(ctx.Resolve<OsuProcessMatchedEvent>().ProcessId);
                if (process == null || process.HasExited) throw new EntryPointNotFoundException();
                return process;
            }).As<Process>();
            return builder;
        }
    }
}
