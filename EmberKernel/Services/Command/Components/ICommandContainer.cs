using EmberKernel.Services.Command.Models;

namespace EmberKernel.Services.Command.Components
{
    public interface ICommandContainer
    {
        bool TryAssignCommand(CommandArgument argument, out CommandArgument newArgument);
    }
}
