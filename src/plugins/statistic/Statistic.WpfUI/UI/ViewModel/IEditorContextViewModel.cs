using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.Hub;
using Statistic.WpfUI.UI.Model;
using System;
using System.ComponentModel;
using System.Windows;

namespace Statistic.WpfUI.UI.ViewModel
{
    public interface IEditorContextViewModel: INotifyPropertyChanged
    {
        IStatisticHub Formats { get; set; }
        IDataSource Variables { get; set; }
        InEditHubFormat EditingHubFormat { get; set; }
        HubFormat SelectedHubFormat { get; set; }
        Visibility CreateVisibility { get; set; }
        Visibility SaveVisibility { get; set; }
        Visibility DeleteVisibility { get; set; }
        EditorMode Mode { get; set; }
        event Action<EditorMode> OnEditorModeChanged;
    }
}
