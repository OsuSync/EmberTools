using Autofac;
using EmberKernel.Services.EventBus;
using EmberKernel.Utils.CommonOutputter;
using EmberKernel.Utils.CommonOutputter.DefaultImpls;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public static class UtilsBuilderHelper
    {
        public static KernelBuilder UseDeafaultUtils(this KernelBuilder builder)
        {
            builder.Configure((context, container) =>
            {
                container.RegisterType<DefaultOutputterFactory>().As<ICommonOutputterFactory>().SingleInstance();
            });
            return builder;
        }
    }
}
