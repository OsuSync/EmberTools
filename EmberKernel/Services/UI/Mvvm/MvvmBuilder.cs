using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
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
            kernelBuilder._containerBuilder
                .RegisterType<ConfigurationModelManager>()
                .AsSelf()
                .As<IConfigurationModelManager>()
                .SingleInstance();
            return this;
        }
    }
}
