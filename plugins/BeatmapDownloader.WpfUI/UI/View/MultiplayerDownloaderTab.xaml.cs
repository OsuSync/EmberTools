using Autofac;
using BeatmapDownloader.Database.Database;
using BeatmapDownloader.WpfUI.UI.ViewModel;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using System;
using System.Collections.Generic;
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

namespace BeatmapDownloader.WpfUI.UI.View
{
    /// <summary>
    /// Interaction logic for MultiplayerDownloaderTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    public partial class MultiplayerDownloaderTab : UserControl, IViewComponent
    {
        public MultiplayerDownloaderTab()
        {
            InitializeComponent();
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            this.DataContext = scope.Resolve<DownloadHistoryViewModel>();
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }

        public void Dispose() { }
    }
}
