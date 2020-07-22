using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using System.Threading.Tasks;

namespace PpCalculator
{
    [EmberPlugin(Author = "ZeroAsh", Name = "PpCalculator", Version = "0.1")]
    public class PpCalculatorPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            throw new System.NotImplementedException();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            throw new System.NotImplementedException();
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            throw new System.NotImplementedException();
        }
    }
}
