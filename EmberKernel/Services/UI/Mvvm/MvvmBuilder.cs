using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
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
                .As<IConfigurationModelManager>()
                .SingleInstance();

            kernelBuilder._containerBuilder
                .RegisterType<KernelViewComponentManager>()
                .As<IViewComponentManager>()
                .SingleInstance();
            return this;
        }
    }
}
