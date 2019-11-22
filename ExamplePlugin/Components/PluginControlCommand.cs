using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Components
{
    class PluginControlCommand : IComponent, ICommandContainer
    {
        private readonly IPluginsManager _pluginMan;

        public PluginControlCommand(IPluginsManager pluginMan)
        {
            _pluginMan = pluginMan;
        }

        [CommandHandler(Command = "enable")]
        public void EnablePlugin(string plugin)
        {
            _pluginMan.Load(FindPlugin(plugin));
        }

        [CommandHandler(Command = "disable")]
        public void DisablePlugin(string plugin)
        {
            _pluginMan.Unload(FindPlugin(plugin));
        }

        public IPlugin FindPlugin(string plugin)
        {
            foreach (var descriptor in _pluginMan.LoadedPlugins())
            {
                if (descriptor.Name == plugin)
                {
                    return _pluginMan.GetPluginByDescriptor(descriptor);
                }
            }
            return null;
        }

        public void Dispose()
        {
        }
    }
}
