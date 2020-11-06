using EmberKernel.Services.EventBus;
using Statistic.Abstract.Events;
using Statistic.WpfUI.UI.ViewModel;
using System;

namespace Statistic.WpfUI.UI.Command
{
    public class DeleteCommand : IEditorContextCommand
    {
        public IEditorContextViewModel ViewModel { get; set; }
        private IEventBus EventBus { get; set; }

        public DeleteCommand(IEditorContextViewModel viewModel, IEventBus eventBus)
        {
            ViewModel = viewModel;
            ViewModel.OnEditorModeChanged += ViewModel_OnEditorModeChanged;
            EventBus = eventBus;
        }

        private void ViewModel_OnEditorModeChanged(EditorMode obj)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => ViewModel.Mode != EditorMode.Idle && !ViewModel.EditingHubFormat.IsCreated;

        public void Execute(object parameter)
        {
            ViewModel.Mode = EditorMode.Idle;
            if (!ViewModel.EditingHubFormat.IsCreated)
            {
                ViewModel.Formats.Unregister(ViewModel.EditingHubFormat.Name);
                EventBus.Publish(new FormatDeletedEvent()
                {
                    Name = ViewModel.EditingHubFormat.Name,
                });
            }
            ViewModel.EditingHubFormat = null;

        }
    }
}
