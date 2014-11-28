using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Emulation.Debugger.MVVM
{
    internal abstract partial class ViewModel : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler propertyChangedHandler;

        protected void PropertyChanged(string propertyName)
        {
            var handler = propertyChangedHandler;
            if (handler != null)
            {
                handler(this, PropertyChangedEventArgsCache.GetOrCreate(propertyName));
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
