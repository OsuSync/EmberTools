using Autofac;
using EmberKernel.Services.UI.Mvvm.Model.Configuration;
using EmberKernel.Services.UI.Mvvm.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm
{
    public class MvvmBuilder : IMvvmBuilder
    {
        internal KernelBuilder kernelBuilder;
        public MvvmBuilder(KernelBuilder kernelBuilder)
        {
            this.kernelBuilder = kernelBuilder;
        }

        public IMvvmBuilder UseConfigurationModel()
        {
            kernelBuilder._containerBuilder.RegisterType<ConfigurationModelManager>().As<IConfigurationModelManager>().SingleInstance();
            return this;
        }
    }
}
