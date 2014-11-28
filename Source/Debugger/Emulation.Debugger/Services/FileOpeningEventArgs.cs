namespace Emulation.Debugger.Services
{
    public class FileOpeningEventArgs : FileEventArgs
    {
        public FileOpeningEventArgs(string filePath)
            : base(filePath)
        {
        }
    }
}
