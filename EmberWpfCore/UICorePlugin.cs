using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberWpfCore.Components.PluginsManager.View;
using EmberWpfCore.View;
using EmberWpfCore.ViewModel;
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
            builder.ConfigureComponent<RegisteredTabs>();
            builder.ConfigureWpfWindow<Main>();
            builder.ConfigureUIComponent<PluginsTab>();
        }

        public override async Task Initialize(ILifetimeScope scope)
        {
            await scope.InitializeWpfWindow<Main>();
            scope.InitializeUIComponent<PluginsTab>();
        }

        public override async Task Uninitialize(ILifetimeScope scope)
        {
            await scope.UninitializeWpfWindow<Main>();
            scope.UninitializeUIComponent<PluginsTab>();
        }
    }
}
