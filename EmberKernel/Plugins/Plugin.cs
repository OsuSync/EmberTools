using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EmberKernel.Plugins.Components;

namespace EmberKernel.Plugins
{
    public abstract class Plugin : IPlugin
    {
        public abstract void BuildComponents(IComponentBuilder builder);
        public abstract Task Initialize(ILifetimeScope scope);
        public abstract Task Uninitialize();
    }
}
