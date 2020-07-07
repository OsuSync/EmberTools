using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;

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
