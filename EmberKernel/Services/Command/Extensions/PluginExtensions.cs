using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Components;

namespace EmberKernel
{
    public static class PluginExtensions
    {
        public static void UseCommandContainer<T>(this ILifetimeScope scope, bool enableCommandHelp = true) where T : ICommandContainer
        {
            ICommandService commandService = scope.Resolve<ICommandService>();
            commandService.RegisterCommandContainer(scope.Resolve<T>(), enableCommandHelp);
        }

        public static void RemoveCommandContainer<T>(this ILifetimeScope scope) where T : ICommandContainer
        {
            ICommandService commandService = scope.Resolve<ICommandService>();
            commandService.UnregisterCommandContainer(scope.Resolve<T>());
        }

        public static void ConfigureCommandContainer<T>(this IComponentBuilder _builder) where T : IComponent
        {
            var builder = _builder as ComponentBuilder;
            builder.Container.RegisterType<T>().SingleInstance();
        }

        public static void ConfugreCommandHelpGenerator<T>(this IComponentBuilder _builder) where T : IComponent
        {
            var builder = _builder as ComponentBuilder;
        }
    }
}
