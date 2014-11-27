using System;
using System.ComponentModel.Composition;
using System.IO;
using Emulation.Core;

namespace Emulation.Debugger.Services
{
    [Export]
    internal class FileService
    {
        private string filePath;
        private Memory memory;

        public string FilePath => this.filePath;

        public bool IsFileOpen => this.filePath != null;

        public Memory Memory => this.memory;

        public void CloseFile()
        {
            var localFilePath = this.filePath;

            FileClosing?.Invoke(this, new FileClosingEventArgs(localFilePath));

            this.filePath = null;
            this.memory = null;

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

            using (var stream = File.OpenRead(filePath))
            {
                this.memory = Memory.CreateFromStream(stream);
            }

            FileOpened?.Invoke(this, new FileOpenedEventArgs(filePath));
        }

        public event EventHandler<FileOpenedEventArgs> FileOpened;
        public event EventHandler<FileOpeningEventArgs> FileOpening;
        public event EventHandler<FileClosedEventArgs> FileClosed;
        public event EventHandler<FileClosingEventArgs> FileClosing;
    }
}
