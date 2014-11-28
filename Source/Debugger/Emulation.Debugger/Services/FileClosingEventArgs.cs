namespace Emulation.Debugger.Services
{
    public class FileClosingEventArgs : FileEventArgs
    {
        public FileClosingEventArgs(string filePath)
            : base(filePath)
        {
        }
    }
}
