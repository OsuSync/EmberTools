using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI;
using EmberKernel.Services.UI.Mvvm.Extension;
using EmberWpfCore.Components.Configuration.View;
using EmberWpfCore.Components.PluginsManager.View;
using EmberWpfCore.View;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmberWpfCore
{
    [EmberPlugin(Author = "yf_extension, Deliay", Name = "CoreUI", Version = "0.0.1")]
    public class UICorePlugin : Plugin, ICoreWpfPlugin
    {
        public UICorePlugin()
        {

        }

        public void BuildApplication(Application application)
        {
            // do load resource stuff here
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<RegisteredTabs>();
            builder.ConfigureWpfWindow<Main>();
            builder.ConfigureUIComponent<PluginsTab>();
            builder.ConfigureUIComponent<ConfigurationTab>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.InitializeWpfWindow<Main>();
            await scope.InitializeUIComponent<PluginsTab>();
            await scope.InitializeUIComponent<ConfigurationTab>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UninitializeWpfWindow<Main>();
            await scope.UninitializeUIComponent<PluginsTab>();
            await scope.UninitializeUIComponent<ConfigurationTab>();
        }
    }
}
