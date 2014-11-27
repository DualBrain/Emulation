using System;
using System.Collections.Generic;

namespace Emulation.Core.Collections
{
    public class PagedCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        private readonly PagedList<T> items;

        public PagedCollection(int capacity = 0, int pageSize = PagedList<T>.DefaultPageSize)
        {
            this.items = new PagedList<T>(capacity, pageSize);
        }

        protected PagedList<T> Items => this.items;

        public int Count => this.items.Count;
        bool ICollection<T>.IsReadOnly => false;

        protected virtual void ClearItems()
        {
            this.items.Clear();
        }

        protected virtual void InsertItem(int index, T item)
        {
            this.items.Insert(index, item);
        }

        protected virtual void RemoveItem(int index)
        {
            this.items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            this.items[index] = item;
        }

        public void Add(T item)
        {
            InsertItem(this.items.Count, item);
        }

        public void Clear()
        {
            ClearItems();
        }

        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(T[] array, int index)
        {
            this.items.CopyTo(array, index);
        }

        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > this.items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            InsertItem(index, item);
        }

        public bool Remove(T item)
        {
            var index = this.items.IndexOf(item);
            if (index >= 0)
            {
                RemoveItem(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            RemoveItem(index);
        }

        public T this[int index]
        {
            get { return this.items[index]; }
            set
            {
                if (index < 0 || index >= this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                SetItem(index, value);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
    }
}
