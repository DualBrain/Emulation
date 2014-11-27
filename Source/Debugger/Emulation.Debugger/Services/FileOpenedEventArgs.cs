namespace Emulation.Debugger.Services
{
    public class FileOpenedEventArgs : FileEventArgs
    {
        public FileOpenedEventArgs(string filePath)
            : base(filePath)
        {
        }
    }
}
