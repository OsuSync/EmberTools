using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using ExamplePlugin.Components;
using ExamplePlugin.Models;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin - 2", Version = "1.0")]
    public class MyPlugin2 : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureCommandContainer<PluginControlCommand>();
            builder.UseConfigurationModel<MyPluginConfiguration>("MyPlugin");
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.UseCommandContainer<PluginControlCommand>();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.RemoveCommandContainer<PluginControlCommand>();
            return default;
        }
    }
}
