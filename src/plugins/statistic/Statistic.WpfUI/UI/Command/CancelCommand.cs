using Statistic.WpfUI.UI.ViewModel;
using System;

namespace Statistic.WpfUI.UI.Command
{
    public class CancelCommand : IEditorContextCommand
    {
        public IEditorContextViewModel ViewModel { get; set; }

        public CancelCommand(IEditorContextViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.OnEditorModeChanged += ViewModel_OnEditorModeChanged;
        }

        private void ViewModel_OnEditorModeChanged(EditorMode obj)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => ViewModel.Mode != EditorMode.Idle;

        public void Execute(object parameter)
        {
            ViewModel.Mode = EditorMode.Idle;
            ViewModel.EditingHubFormat = null;
        }
    }
}
