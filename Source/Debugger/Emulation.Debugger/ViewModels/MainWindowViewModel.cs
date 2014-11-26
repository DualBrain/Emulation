using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Emulation.Debugger.MVVM;

namespace Emulation.Debugger.ViewModels
{
    [Export]
    internal class MainWindowViewModel : ViewModel<Window>
    {
        private MainWindowViewModel()
            : base("MainWindowView")
        {
            this.ExitCommand = RegisterCommand("Exit", "Exit", ExitCommandExecuted, CanExitCommandExecute);
        }

        private void ExitCommandExecuted()
        {
            this.View.Close();
        }

        private bool CanExitCommandExecute()
        {
            return true;
        }

        public ICommand ExitCommand { get; }

        public string Title => "Debugger";
    }
}
