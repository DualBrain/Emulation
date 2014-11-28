using System;
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

            this.lines = new BulkObservableCollection<MemoryLineViewModel>(pageSize: 4096);
            this.readOnlyLines = lines.AsReadOnly();
        }

        private void FileOpened(object sender, FileOpenedEventArgs e)
        {
            var memory = this.fileService.Memory;
            var size = memory.Size;
            var hexWidth = (int)Math.Log10(memory.Size);

            this.lines.BeginBulkOperation();
            try
            {
                this.lines.EnsureCapacity((size / 8) + 1);

                for (int address = 0; address < size; address += 8)
                {
                    this.lines.Add(new MemoryLineViewModel(memory, address, hexWidth));
                }
            }
            finally
            {
                this.lines.EndBulkOperation();
            }

            PropertyChanged(nameof(HasData));
        }

        private void FileClosed(object sender, FileClosedEventArgs e)
        {
            this.lines.Clear();

            PropertyChanged(nameof(HasData));
        }

        public bool HasData => this.lines.Count > 0;

        public ReadOnlyBulkObservableCollection<MemoryLineViewModel> Lines => readOnlyLines;
    }
}
