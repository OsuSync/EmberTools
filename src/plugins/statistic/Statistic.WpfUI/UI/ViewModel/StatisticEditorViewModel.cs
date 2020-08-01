using Autofac;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.Hub;
using Statistic.WpfUI.UI.Command;
using Statistic.WpfUI.UI.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace Statistic.WpfUI.UI.ViewModel
{
    public class StatisticEditorViewModel : IComponent, IEditorContextViewModel
    {
        public IStatisticHub Formats { get; set; }
        public IDataSource Variables { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<EditorMode> OnEditorModeChanged;

        public ICommand CreateFormat { get; private set; }
        public ICommand SaveFormat { get; private set; }
        public ICommand CancelFormat { get; private set; }
        private readonly ILifetimeScope _viewModeScope;
            
        public StatisticEditorViewModel(ILifetimeScope scope)
        {
            MoveTo(EditorMode.Idle);
            _viewModeScope = scope.BeginLifetimeScope((builder) =>
            {
                builder.RegisterInstance(this).As<IEditorContextViewModel>().SingleInstance();
                builder.RegisterType<NewFormatCommand>().As<IEditorContextCommand>().Named<IEditorContextCommand>("Create");
                builder.RegisterType<SaveFormatCommand>().As<IEditorContextCommand>().Named<IEditorContextCommand>("Save");
                builder.RegisterType<CancelCommand>().As<IEditorContextCommand>().Named<IEditorContextCommand>("Cancel");
            });
            CreateFormat = _viewModeScope.ResolveNamed<IEditorContextCommand>("Create");
            SaveFormat = _viewModeScope.ResolveNamed<IEditorContextCommand>("Save");
            CancelFormat = _viewModeScope.ResolveNamed<IEditorContextCommand>("Cancel");
        }


        private HubFormat _selectedHubFormat;
        public HubFormat SelectedHubFormat
        {
            get => _selectedHubFormat;
            set
            {
                if (Equals(_selectedHubFormat, value)) return;
                _selectedHubFormat = value;
                OnPropertyChanged();
            }
        }

        private InEditHubFormat _editingHubFormat;
        public InEditHubFormat EditingHubFormat
        {
            get => _editingHubFormat;
            set
            {
                if (Equals(_editingHubFormat, value)) return;
                _editingHubFormat = value;
                OnPropertyChanged();
            }
        }
        public Visibility CreateVisibility { get; set; }
        public Visibility SaveVisibility { get; set; }
        public Visibility DeleteVisibility { get; set; }
        public Visibility CancelVisibility { get; set; }
        public Visibility EditorVisibility { get; set; }
        private EditorMode _mode;
        public EditorMode Mode
        {
            get => _mode;
            set
            {
                if (value == _mode) return;
                _mode = value;
                MoveTo(_mode);
                OnEditorModeChanged?.Invoke(_mode);
                OnPropertyChanged();
            }
        }
        private void MoveTo(EditorMode mode)
        {
            switch (mode)
            {
                case EditorMode.Idle:
                    CreateVisibility = Visibility.Visible;
                    SaveVisibility = Visibility.Collapsed;
                    DeleteVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Collapsed;
                    EditorVisibility = Visibility.Hidden;
                    break;
                case EditorMode.Creating:
                    CreateVisibility = Visibility.Collapsed;
                    SaveVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Visible;
                    EditorVisibility = Visibility.Visible;
                    break;
                case EditorMode.Editing:
                    CreateVisibility = Visibility.Collapsed;
                    SaveVisibility = Visibility.Visible;
                    DeleteVisibility = Visibility.Visible;
                    CancelVisibility = Visibility.Visible;
                    EditorVisibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            OnPropertyChanged(nameof(CreateVisibility));
            OnPropertyChanged(nameof(SaveVisibility));
            OnPropertyChanged(nameof(DeleteVisibility));
            OnPropertyChanged(nameof(CancelVisibility));
            OnPropertyChanged(nameof(EditorVisibility));
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _viewModeScope.Dispose();
        }
    }
}
