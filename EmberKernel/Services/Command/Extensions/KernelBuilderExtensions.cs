using EmberKernel.Services.Command;

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
