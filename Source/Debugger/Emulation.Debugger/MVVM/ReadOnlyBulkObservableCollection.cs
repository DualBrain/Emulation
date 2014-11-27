using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Emulation.Debugger.MVVM
{
    public class ReadOnlyBulkObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ReadOnlyBulkObservableCollection(BulkObservableCollection<T> collection)
            : base(collection)
        {
            collection.CollectionChanged += HandleCollectionChanged;
            collection.PropertyChanged += HandlePropertyChanged;
        }

        protected virtual void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
