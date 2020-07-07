using Autofac;
using EmberKernel.Services.Command.Sources;
using System;

namespace EmberKernel.Services.Command.Builder
{
    internal class CommandSourceBuilder : ICommandSourceBuilder
    {
        private ContainerBuilder Container { get; }
        public CommandSourceBuilder(ContainerBuilder container)
        {
            Container = container;
        }

        public ICommandSourceBuilder ConfigureMultiSource()
        {
            throw new NotImplementedException("Multi source is not implemeneted");
        }

        public ICommandSourceBuilder ConfigureSource<T>()
        {
            Container.RegisterType<T>().As<ICommandSource>().SingleInstance();
            return this;
        }

        public ICommandSourceBuilder ConfigureSource(Type type)
        {
            if (!typeof(ICommandSource).IsAssignableFrom(type))
            {
                throw new TypeLoadException($"Pass type in {nameof(type)} is not inherited ICommandSource");
            }
            Container.RegisterType(type).As<ICommandSource>().SingleInstance();
            return this;
        }
    }
}
