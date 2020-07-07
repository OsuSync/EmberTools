using Autofac;
using EmberKernel.Plugins.Components;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPlugin
    {
        void BuildComponents(IComponentBuilder builder);
        ValueTask Initialize(ILifetimeScope scope);
        ValueTask Uninitialize(ILifetimeScope scope);
    }
}
