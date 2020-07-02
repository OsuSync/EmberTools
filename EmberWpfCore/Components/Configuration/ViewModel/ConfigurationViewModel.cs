using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private IConfigurationModelManager ConfigurationModelManager { get; }
        //private IWindowManager WindowManager { get; }
        private Dictionary<Type, ConfigurationSetViewModel> ViewModels = new Dictionary<Type, ConfigurationSetViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ConfigurationSetViewModel> ConfigurationSets { get; } = new ObservableCollection<ConfigurationSetViewModel>();
        private ConfigurationSetViewModel _selectSet = null;
        public ConfigurationSetViewModel SelectedSet {
            get => _selectSet;
            set
            {
                if (Equals(_selectSet, value)) return;
                _selectSet = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedSet));
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ConfigurationViewModel(IConfigurationModelManager configurationModelManager) //, IWindowManager windowManager)
        {
            ConfigurationModelManager = configurationModelManager;
            //WindowManager = windowManager;
            LoadConfigurationModels(ConfigurationModelManager);
            ConfigurationModelManager.CollectionChanged += ConfigurationModelManager_CollectionChanged;
        }

        private void ConfigurationModelManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {   
                case NotifyCollectionChangedAction.Add:
                    LoadConfigurationModels(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        ViewModels.Remove(item.GetType());
                        ConfigurationSets.Remove(ConfigurationSets.First(vm => vm.ConfigurationType == item.GetType()));
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadConfigurationModels(IEnumerable handleEnumerable)
        {
            foreach (var item in handleEnumerable)
            {
                HandleTypeDependency(item);
            }
        }

        private void HandleTypeDependency(object item)
        {
            var itemType = item.GetType();
            if (!(item is DependencySet dependency)) return;
            if (ViewModels.ContainsKey(itemType)) return;
            var vm = new ConfigurationSetViewModel(dependency);
            ViewModels.Add(itemType, vm);
            vm.LoadDependencySet();
            vm.CollectionChanged += Vm_CollectionChanged;
            ConfigurationSets.Add(vm);
            //WindowManager.BeginUIThreadScope(() => ConfigurationSets.Add(vm));
        }
        private void Vm_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ConfigurationSets));
        }
    }
}
