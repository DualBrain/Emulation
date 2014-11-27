using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Emulation.Debugger.MVVM;
using Emulation.Debugger.Services;
using Microsoft.Win32;

namespace Emulation.Debugger.ViewModels
{
    [Export]
    internal class MainWindowViewModel : ViewModel<Window>
    {
        private readonly FileService fileService;

        [ImportingConstructor]
        private MainWindowViewModel(FileService fileService)
            : base("MainWindowView")
        {
            this.fileService = fileService;

            this.ExitCommand = RegisterCommand("Exit", "Exit", ExitCommandExecuted, CanExitCommandExecute);
            this.OpenFileCommand = RegisterCommand("Open File", "OpenFile", OpenFileCommandExecuted, CanOpenFileCommandExecute, new KeyGesture(Key.O, ModifierKeys.Control));
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
            var dialog = new OpenFileDialog
            {
                Title = "Open File"
            };

            if (dialog.ShowDialog(this.View) == true)
            {
                this.fileService.OpenFile(dialog.FileName);
                PropertyChanged("Title");
            }
        }

        public ICommand ExitCommand { get; }
        public ICommand OpenFileCommand { get; }

        public string Title =>
            this.fileService.IsFileOpen
                ? "Debugger - \{Path.GetFileName(this.fileService.FilePath)}"
                : "Debugger";
    }
}
