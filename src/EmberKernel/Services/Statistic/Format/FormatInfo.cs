using Autofac;
using System;

namespace EmberKernel.Services.Statistic.Format
{
    public class FormatInfo
    {
        public string Format { get; }
        internal ILifetimeScope Scope { get; }
        public Type ContainerType { get; }

        public FormatInfo(string Format, ILifetimeScope Scope, Type ContainerType)
        {
            this.Scope = Scope;
            this.ContainerType = ContainerType;
            this.Format = Format;
        }
    }

    public class FormatInfo<T> : FormatInfo where T : IFormatContainer
    {
        public FormatInfo(string Format, ILifetimeScope Scope) : base(Format, Scope, typeof(T))
        {

        }
    }
}
