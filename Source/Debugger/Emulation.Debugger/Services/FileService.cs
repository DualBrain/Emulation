using System;
using System.ComponentModel.Composition;

namespace Emulation.Debugger.Services
{
    [Export]
    internal class FileService
    {
        private string filePath;

        public string FilePath => this.filePath;

        public bool IsFileOpen => this.filePath != null;

        public void CloseFile()
        {
            var localFilePath = this.filePath;

            FileClosing?.Invoke(this, new FileClosingEventArgs(localFilePath));

            this.filePath = null;

            FileClosed?.Invoke(this, new FileClosedEventArgs(localFilePath));
        }

        public void OpenFile(string filePath)
        {
            if (IsFileOpen)
            {
                CloseFile();
            }

            FileOpening?.Invoke(this, new FileOpeningEventArgs(filePath));

            this.filePath = filePath;

            FileOpened?.Invoke(this, new FileOpenedEventArgs(filePath));
        }

        public event EventHandler<FileOpenedEventArgs> FileOpened;
        public event EventHandler<FileOpeningEventArgs> FileOpening;
        public event EventHandler<FileClosedEventArgs> FileClosed;
        public event EventHandler<FileClosingEventArgs> FileClosing;
    }
}
