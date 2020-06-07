using Autofac;
using EmberKernel.Services.UI.Mvvm;
using EmberKernel.Services.UI.Mvvm.Model;
using EmberKernel.Services.UI.Mvvm.View;
using EmberKernel.Services.UI.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Extension
{
    public static class KernelBuilderExtension
    {
        public static KernelBuilder UseMvvmInterface(this KernelBuilder kernelBuilder, Action<IMvvmBuilder> builder)
        {
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder.RegisterType<KernelModelManager>().As<IModelManager>().SingleInstance());
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder.RegisterType<KernelViewManager>().As<IViewManager>().SingleInstance());
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder.RegisterType<KernelViewModelManager>().As<IViewModelManager>().SingleInstance());
            builder(new MvvmBuilder(kernelBuilder));
            return kernelBuilder;
        }
    }
}
