using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Emulation.Debugger.MVVM
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        private static Dictionary<string, PropertyChangedEventArgs> eventArgsCache;
        private static object gate = new object();

        private static PropertyChangedEventArgs GetEventArgs(string propertyName)
        {
            PropertyChangedEventArgs result;

            lock (gate)
            {
                if (eventArgsCache == null)
                {
                    eventArgsCache = new Dictionary<string, PropertyChangedEventArgs>();
                }

                if (!eventArgsCache.TryGetValue(propertyName, out result))
                {
                    result = new PropertyChangedEventArgs(propertyName);
                    eventArgsCache.Add(propertyName, result);
                }
            }

            return result;
        }

        private PropertyChangedEventHandler propertyChangedHandler;

        protected void PropertyChanged(string propertyName)
        {
            var handler = propertyChangedHandler;
            if (handler != null)
            {
                handler(this, GetEventArgs(propertyName));
            }
        }

        protected void AllPropertiesChanged()
        {
            PropertyChanged(string.Empty);
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged(propertyName);
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChangedHandler += value; }
            remove { propertyChangedHandler -= value; }
        }
    }
}
