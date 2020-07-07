using System.Threading.Tasks;

namespace EmberKernel.Plugins.Components
{
    public interface IEntryComponent : IComponent
    {
        ValueTask Start();
    }
}
