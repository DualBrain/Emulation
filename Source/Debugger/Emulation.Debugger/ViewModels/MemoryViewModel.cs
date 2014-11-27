using System.ComponentModel.Composition;
using System.Windows.Controls;
using Emulation.Debugger.MVVM;
using Emulation.Debugger.Services;

namespace Emulation.Debugger.ViewModels
{
    [Export]
    internal class MemoryViewModel : ViewModel<UserControl>
    {
        private readonly FileService fileService;

        private readonly BulkObservableCollection<MemoryLineViewModel> lines;
        private readonly ReadOnlyBulkObservableCollection<MemoryLineViewModel> readOnlyLines;

        [ImportingConstructor]
        private MemoryViewModel(FileService fileService)
            : base("MemoryView")
        {
            this.fileService = fileService;

            this.fileService.FileOpened += FileOpened;
            this.fileService.FileClosed += FileClosed;

            this.lines = new BulkObservableCollection<MemoryLineViewModel>();
            this.readOnlyLines = lines.AsReadOnly();
        }

        private void FileOpened(object sender, FileOpenedEventArgs e)
        {
            var memory = this.fileService.Memory;
            var size = memory.Size;

            this.lines.BeginBulkOperation();
            try
            {
                for (int i = 0; i < size; i += 8)
                {
                    this.lines.Add(new MemoryLineViewModel(memory, i));
                }
            }
            finally
            {
                this.lines.EndBulkOperation();
            }
        }

        private void FileClosed(object sender, FileClosedEventArgs e)
        {
            this.lines.Clear();
        }

        public ReadOnlyBulkObservableCollection<MemoryLineViewModel> Lines => readOnlyLines;
    }
}
