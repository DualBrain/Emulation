using System;

namespace Emulation.Debugger.Services
{
    public abstract class FileEventArgs : EventArgs
    {
        private readonly string filePath;

        protected FileEventArgs(string filePath)
        {
            this.filePath = filePath;
        }

        public string FilePath => filePath;
    }
}