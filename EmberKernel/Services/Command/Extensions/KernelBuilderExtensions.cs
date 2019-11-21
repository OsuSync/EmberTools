using EmberKernel.Services.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public static class KernelBuilderExtensions
    {
        public static KernelBuilder UseCommandService(this KernelBuilder builder)
        {
            builder.UseKernalService<CommandService, ICommandService>();
            return builder;
        }
    }
}
