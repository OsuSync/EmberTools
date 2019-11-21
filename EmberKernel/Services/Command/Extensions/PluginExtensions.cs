using Autofac;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public static class PluginExtensions
    {
        public static void UseCommandContainer<T>(this ILifetimeScope scope) where T : ICommandContainer
        {
            ICommandService commandService = scope.Resolve<ICommandService>();
            commandService.ReigsterCommandContainer(scope.Resolve<T>());
        }
    }
}
