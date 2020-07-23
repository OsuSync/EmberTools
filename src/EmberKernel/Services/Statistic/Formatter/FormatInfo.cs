using Autofac;
using System;

namespace EmberKernel.Services.Statistic.Format
{
    public class FormatInfo
    {
        internal ILifetimeScope Scope { get; }
        public Type ContainerType { get; }

        public FormatInfo(ILifetimeScope Scope, Type ContainerType)
        {
            this.Scope = Scope;
            this.ContainerType = ContainerType;
        }
    }
}
