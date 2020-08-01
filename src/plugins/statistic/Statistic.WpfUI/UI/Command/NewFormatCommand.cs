using Statistic.WpfUI.UI.Model;
using Statistic.WpfUI.UI.ViewModel;
using System;

namespace Statistic.WpfUI.UI.Command
{
    public class NewFormatCommand : IEditorContextCommand
    {
        public IEditorContextViewModel ViewModel { get; set; }

        public NewFormatCommand(IEditorContextViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.OnEditorModeChanged += ViewModel_OnEditorModeChanged;
        }

        public event EventHandler CanExecuteChanged;
        private void ViewModel_OnEditorModeChanged(EditorMode obj)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool CanExecute(object parameter) => ViewModel.Mode == EditorMode.Idle;

        private string RandomName() => $"pattern_{(new Random()).Next(100000)}";
        public void Execute(object parameter)
        {
            var randomName = RandomName();
            while (ViewModel.Formats.IsRegistered(randomName)) randomName = RandomName();
            ViewModel.EditingHubFormat = new InEditHubFormat(ViewModel.Formats)
            {
                Name = randomName,
                IsCreated = false,
                Original = null,
            };
            ViewModel.Mode = EditorMode.Creating;
        }
    }
}
