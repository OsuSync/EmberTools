using Autofac;
using EmberKernel.Services.UI.Mvvm;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberKernel.Services.UI.Mvvm.ViewModel;
using System;

namespace EmberKernel.Services.UI.Extension
{
    public static class KernelBuilderExtension
    {
        public static KernelBuilder UseMvvmInterface(this KernelBuilder kernelBuilder, Action<IMvvmBuilder> builder)
        {
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder.RegisterType<KernelViewModelManager>().As<IViewModelManager>().SingleInstance());
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder.RegisterType<KernelViewComponentManager>().As<IViewComponentManager>().SingleInstance());
            builder(new MvvmBuilder(kernelBuilder));
            return kernelBuilder;
        }

        public static KernelBuilder UseWindowManager<T, TWindow>(this KernelBuilder kernelBuilder)
            where T : IWindowManager<TWindow>
        {
            kernelBuilder.buildActions.Add(() => kernelBuilder._containerBuilder
                .RegisterType<T>()
                .AsSelf()
                .As<IWindowManager>()
                .As<IWindowManager<TWindow>>()
                .SingleInstance());
            return kernelBuilder;
        }
    }
}
