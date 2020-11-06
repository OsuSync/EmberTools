using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using System;
using System.Threading.Tasks;

namespace EmberLyricDisplayerPlugin
{
    [EmberPlugin(Author = "MikiraSora", Name = "EmberLyricDisplayerPlugin", Version = "0.7.5")]
    public class EmberLyricDisplayerPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {

        }

        public override Task Initialize(ILifetimeScope scope)
        {

            scope.Subscription<PlayingInfo, MemoryReaderHandler>();
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            throw new NotImplementedException();
        }
    }
}
