using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Emulation.Debugger.Extensions;
using Emulation.Debugger.MVVM;
using Emulation.Debugger.Services;
using Microsoft.Win32;

namespace Emulation.Debugger.ViewModels
{
    [Export]
    internal class MainWindowViewModel : ViewModel<Window>
    {
        private readonly FileService fileService;
        private readonly MemoryViewModel memoryViewModel;

        [ImportingConstructor]
        private MainWindowViewModel(
            FileService fileService,
            MemoryViewModel memoryViewModel)
            : base("MainWindowView")
        {
            this.fileService = fileService;
            this.memoryViewModel = memoryViewModel;

            this.fileService.FileClosed += FileClosed;
            this.fileService.FileOpened += FileOpened;

            this.ExitCommand = RegisterCommand("Exit", "Exit", ExitCommandExecuted, CanExitCommandExecute);
            this.OpenFileCommand = RegisterCommand("Open File", "OpenFile", OpenFileCommandExecuted, CanOpenFileCommandExecute, new KeyGesture(Key.O, ModifierKeys.Control));
        }

        protected override void OnViewCreated(Window view)
        {
            var memory = view.FindName<Border>("Memory");
            memory.Child = memoryViewModel.CreateView();
        }

        private void FileClosed(object sender, FileClosedEventArgs e)
        {
            PropertyChanged(nameof(Title));
        }

        private void FileOpened(object sender, FileOpenedEventArgs e)
        {
            PropertyChanged(nameof(Title));
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
