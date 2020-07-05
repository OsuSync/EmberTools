using Autofac;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using ExamplePlugin.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamplePlugin.ViewComponents
{
    /// <summary>
    /// Interaction logic for MyTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    [ViewComponentName(name: "Example Custom Tab")]
    public partial class MyTab : UserControl, IViewComponent
    {
        public static IConfigurationModelManager ConfigurationManager { get; set; }
        public MyTab()
        {
            InitializeComponent();
        }

        ConfigurationViewModel ViewMode { get; set; }
        IConfigurationModelManager ModelManager { get; set; }
        public ValueTask Initialize(ILifetimeScope scope)
        {
            var manager = scope.Resolve<IConfigurationModelManager>();
            ConfigurationManager = manager;
            ConfigurationManager.CollectionChanged += Manager_CollectionChanged;
            InitializeDependecy(ConfigurationManager);
            return default;
        }

        private void InitializeDependecy(IEnumerable initilizer)
        {
            foreach (var item in initilizer)
            {
                if (item is DependencySet dependencySet && dependencySet.GetTypeName() == "MyPluginConfiguration")
                {
                    ViewMode = new ConfigurationViewModel(dependencySet);
                    if (FindName("configurations") is ListBox list)
                    {
                        list.ItemsSource = ConfigurationManager;
                    }
                    DataContext = ViewMode;
                }
            }
        }

        private void Manager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                InitializeDependecy(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is DependencySet dependencySet && dependencySet.GetTypeName() == "MyPluginConfiguration")
                    {
                        ViewMode = null;
                        DataContext = null;
                        if (FindName("configurations") is ListBox list)
                        {
                            list.ItemsSource = null;
                        }
                    }
                }
            }
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            if (ConfigurationManager != null) ConfigurationManager.CollectionChanged -= Manager_CollectionChanged;
            return default;
        }

        public void Dispose()
        {
        }
    }
}
