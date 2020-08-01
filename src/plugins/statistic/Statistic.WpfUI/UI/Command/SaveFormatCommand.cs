using EmberKernel.Services.EventBus;
using Statistic.Abstract.Events;
using Statistic.WpfUI.UI.ViewModel;
using System;

namespace Statistic.WpfUI.UI.Command
{
    public class SaveFormatCommand : IEditorContextCommand
    {
        public IEditorContextViewModel ViewModel { get; set; }
        private IEventBus EventBus { get; set; }

        public SaveFormatCommand(IEditorContextViewModel viewModel, IEventBus eventBus)
        {
            ViewModel = viewModel;
            ViewModel.OnEditorModeChanged += ViewModel_OnEditorModeChanged;
            EventBus = eventBus;
        }

        public event EventHandler CanExecuteChanged;

        private void ViewModel_OnEditorModeChanged(EditorMode obj)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => ViewModel.Mode != EditorMode.Idle;

        public void Execute(object parameter)
        {
            ViewModel.Mode = EditorMode.Idle;
            if (ViewModel.EditingHubFormat.IsCreated)
            {
                ViewModel.Formats.Register(ViewModel.EditingHubFormat.Name, ViewModel.EditingHubFormat.Format);
                EventBus.Publish(new FormatCreatedEvent()
                {
                    Format = ViewModel.EditingHubFormat.Format,
                    Name = ViewModel.EditingHubFormat.Name,
                });
            }
            else
            {
                var newName = ViewModel.EditingHubFormat.Original != null && ViewModel.EditingHubFormat.Original?.Name != ViewModel.EditingHubFormat.Name
                    ? ViewModel.EditingHubFormat.Name
                    : null;
                var originalName = ViewModel.EditingHubFormat.Original?.Name ?? ViewModel.EditingHubFormat.Name;
                ViewModel.Formats.Update(originalName, ViewModel.EditingHubFormat.Format, newName);
                EventBus.Publish(new FormatUpdatedEvent()
                {
                    Format = ViewModel.EditingHubFormat.Format,
                    Name = originalName,
                    NewName = newName,
                });
            }
            ViewModel.EditingHubFormat = null;
        }
    }
}
