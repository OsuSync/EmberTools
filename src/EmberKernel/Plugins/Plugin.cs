using Autofac;
using EmberKernel.Plugins.Components;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public abstract class Plugin : IPlugin
    {
        public abstract void BuildComponents(IComponentBuilder builder);

        public abstract ValueTask Initialize(ILifetimeScope scope);
        public abstract ValueTask Uninitialize(ILifetimeScope scope);
    }
}
