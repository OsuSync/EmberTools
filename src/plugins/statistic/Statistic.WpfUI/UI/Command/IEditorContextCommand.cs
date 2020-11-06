using Statistic.WpfUI.UI.ViewModel;
using System.Windows.Input;

namespace Statistic.WpfUI.UI.Command
{
    public interface IEditorContextCommand : ICommand
    {
        IEditorContextViewModel ViewModel { get; set; }
    }
}
