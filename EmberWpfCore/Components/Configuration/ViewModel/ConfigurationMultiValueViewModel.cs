using EmberCore.KernelServices.UI.ViewModel.Configuration;
using EmberKernel.Services.UI.Mvvm.Dependency;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationMultiValueViewModel : ConfigurationSingleValueViewModel, INotifyCollectionChanged
    {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public object ItemSource { get; }

        public ConfigurationMultiValueViewModel(string name, DependencySet dependencySet)
            : base(name, dependencySet)
        {
            var attachMap = dependencySet.GetAttach<Dictionary<string, object>>(WpfFeatureBuilder.WpfExtraData);
            ItemSource = attachMap[name];
            (ItemSource as INotifyCollectionChanged).CollectionChanged += ConfigurationMultiValueViewModel_CollectionChanged;
        }

        private void ConfigurationMultiValueViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
