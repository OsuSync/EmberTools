using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using ExamplePlugin.ViewComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Example GUI Plugin - 3", Version = "1.0")]
    class MyGuiPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureUIComponent<MyTab>();
        }

        public override Task Initialize(ILifetimeScope scope)
        {
            scope.InitializeUIComponent<MyTab>();
            return Task.CompletedTask;
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            scope.UninitializeUIComponent<MyTab>();
            return Task.CompletedTask;
        }
    }
}
