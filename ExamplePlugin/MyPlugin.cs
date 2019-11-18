using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            
        }

        public override async Task Initialize(ILifetimeScope scope)
        {
            await Task.Yield();
        }

        public override async Task Uninitialize()
        {
            await Task.Yield();
        }
    }
}
