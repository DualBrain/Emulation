using System.ComponentModel.Composition;
using System.Windows;
using Emulation.Debugger.MVVM;

namespace Emulation.Debugger.ViewModels
{
    [Export]
    internal class MainWindowViewModel : ViewModel<Window>
    {
        private MainWindowViewModel()
            : base("MainWindowView")
        {
        }

        public string Title => "Debugger";
    }
}
