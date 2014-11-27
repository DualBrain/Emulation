namespace Emulation.Debugger.Services
{
    public class FileClosedEventArgs : FileEventArgs
    {
        public FileClosedEventArgs(string filePath)
            : base(filePath)
        {
        }
    }
}
