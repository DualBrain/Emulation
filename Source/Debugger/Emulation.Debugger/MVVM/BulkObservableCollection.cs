using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Emulation.Core.Collections;

namespace Emulation.Debugger.MVVM
{
    public partial class BulkObservableCollection<T> : PagedCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private int bulkOperationCount;
        private bool changedDuringBulkOperation;

        public BulkObservableCollection(int capacity = 0, int pageSize = PagedList<T>.DefaultPageSize)
            : base(capacity, pageSize)
        {
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, PropertyChangedEventArgsCache.GetOrCreate(name));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T item, T oldItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, oldItem, index));
        }

        private void OnCollectionReset()
        {
            OnCountChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        private void OnCountChanged()
        {
            OnPropertyChanged(nameof(Count));
            OnItemsChanged();
        }

        private void OnItemsChanged()
        {
            OnPropertyChanged("Items[]");
        }

        protected override void ClearItems()
        {
            var hadItems = Count != 0;

            base.ClearItems();

            if (hadItems)
            {
                if (bulkOperationCount == 0)
                {
                    OnCollectionReset();
                }
                else
                {
                    changedDuringBulkOperation = true;
                }
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (bulkOperationCount == 0)
            {
                OnCountChanged();
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
            }
            else
            {
                changedDuringBulkOperation = true;
            }
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            base.RemoveItem(index);

            if (bulkOperationCount == 0)
            {
                OnCountChanged();
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
            }
            else
            {
                changedDuringBulkOperation = true;
            }
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this[index];

            base.SetItem(index, item);

            if (bulkOperationCount == 0)
            {
                OnItemsChanged();
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, oldItem, index);
            }
            else
            {
                changedDuringBulkOperation = true;
            }
        }

        public void BeginBulkOperation()
        {
            bulkOperationCount++;
        }

        public void EndBulkOperation()
        {
            if (bulkOperationCount == 0)
            {
                throw new InvalidOperationException("\{nameof(EndBulkOperation)} called without a matching call to \{nameof(BeginBulkOperation)}");
            }

            bulkOperationCount--;

            if (bulkOperationCount == 0 && changedDuringBulkOperation)
            {
                OnCollectionReset();
                changedDuringBulkOperation = false;
            }
        }

        public ReadOnlyBulkObservableCollection<T> AsReadOnly()
        {
            return new ReadOnlyBulkObservableCollection<T>(this);
        }

        public T Find(Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var v in this)
            {
                if (predicate(v))
                {
                    return v;
                }
            }

            return default(T);
        }

        public bool TryFind(Func<T, bool> predicate, out T item)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var v in this)
            {
                if (predicate(v))
                {
                    item = v;
                    return true;
                }
            }

            item = default(T);
            return false;
        }

        public int BinarySearch(int index, int length, T value, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            var low = index;
            var high = (index + length) - 1;

            while (low <= high)
            {
                var mid = low + ((high - low) / 2);
                var comp = comparer.Compare(this[mid], value);

                if (comp == 0)
                {
                    return mid;
                }

                if (comp < 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return ~low;
        }

        public int BinarySearch(int index, int length, Func<T, int> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var low = index;
            var high = (index + length) - 1;

            while (low <= high)
            {
                var mid = low + ((high - low) / 2);
                var comp = comparer(this[mid]);

                if (comp == 0)
                {
                    return mid;
                }

                if (comp < 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return ~low;
        }

        public int BinarySearch(int index, int length, T value, Func<T, T, int> comparer)
        {
            return comparer == null
                ? BinarySearch(index, length, value, Comparer<T>.Default)
                : BinarySearch(index, length, value, new FuncComparer(comparer));
        }

        public int BinarySearch(T value, IComparer<T> comparer)
        {
            return comparer == null
                ? BinarySearch(0, Count, value, Comparer<T>.Default)
                : BinarySearch(0, Count, value, comparer);
        }

        public int BinarySearch(T value, Func<T, T, int> comparer)
        {
            return comparer == null
                ? BinarySearch(0, Count, value, Comparer<T>.Default)
                : BinarySearch(0, Count, value, new FuncComparer(comparer));
        }

        public int BinarySearch(T value)
        {
            return BinarySearch(0, Count, value, Comparer<T>.Default);
        }

        public int BinarySearch(Func<T, int> comparer)
        {
            return BinarySearch(0, Count, comparer);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            BeginBulkOperation();
            try
            {
                var readOnlyList = items as IReadOnlyList<T>;
                if (readOnlyList != null)
                {
                    for (int i = 0; i < readOnlyList.Count; i++)
                    {
                        Add(readOnlyList[i]);
                    }

                    return;
                }

                var list = items as IList<T>;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Add(list[i]);
                    }

                    return;
                }

                foreach (var item in items)
                {
                    Add(item);
                }
            }
            finally
            {
                EndBulkOperation();
            }
        }

        public void EnsureCapacity(int value)
        {
            if (value > Count)
            {
                this.Items.EnsureCapacity(value);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
