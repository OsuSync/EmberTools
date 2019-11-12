using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public interface IScopeBilder
    {
        void BuildScope(ContainerBuilder builder);
        void Run(ILifetimeScope scope);
    }
}
