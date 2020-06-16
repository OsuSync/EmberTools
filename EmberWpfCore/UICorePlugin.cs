using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI;
using EmberWpfCore.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberWpfCore
{
    [EmberPlugin(Author = "Deliay", Name = "CoreUI", Version = "0.0.1")]
    public class UICorePlugin : Plugin
    {
        public UICorePlugin()
        {

        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<WpfUIComponent<Main>>();
        }

        public override Task Initialize(ILifetimeScope scope)
        {
            return Task.CompletedTask;
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            return Task.CompletedTask;
        }
    }
}
