using System.Windows;

namespace Emulation.Debugger.Extensions
{
    internal static class FrameworkElementExtensions
    {
        public static T FindName<T>(this FrameworkElement element, string name)
        {
            return (T)element.FindName(name);
        }
    }
}
