using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Emulation.Debugger.ViewModels;

namespace Emulation.Debugger
{
    public partial class App : Application
    {
        private CompositionContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            this.container = new CompositionContainer(
                new AssemblyCatalog(typeof(App).Assembly));

            var mainWindowViewModel = container.GetExportedValue<MainWindowViewModel>();

            this.MainWindow = mainWindowViewModel.CreateView();
            this.MainWindow.Show();
        }

        public new static App Current => (App)Application.Current;
    }
}
