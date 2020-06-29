using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Listener
{
    public static class ProcessListenerBuilderExtensions
    {
        public static void UseProcessListener(this IComponentBuilder builder, Func<ProcessListenerBuilder, ProcessListenerBuilder> build)
        {
            var processListenerBuilder = new ProcessListenerBuilder(builder);
            build(processListenerBuilder);
            processListenerBuilder.Build();
        }
    }
}
