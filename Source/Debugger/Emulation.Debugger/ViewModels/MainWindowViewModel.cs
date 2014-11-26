using System;
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
            this.OpenFileCommand = RegisterCommand("Open File", "OpenFile", OpenFileCommandExecuted, CanOpenFileCommandExecute);
        }

        private bool CanExitCommandExecute()
        {
            return true;
        }

        private bool CanOpenFileCommandExecute()
        {
            return true;
        }

        private void ExitCommandExecuted()
        {
            this.View.Close();
        }

        private void OpenFileCommandExecuted()
        {
            throw new NotImplementedException();
        }

        public ICommand ExitCommand { get; }
        public ICommand OpenFileCommand { get; }

        public string Title => "Debugger";
    }
}
