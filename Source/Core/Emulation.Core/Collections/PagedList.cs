using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Emulation.Core.Collections
{
    public partial class PagedList<T> : IList<T>, IReadOnlyList<T>
    {
        public const int DefaultPageSize = 16;

        private int count;
        private T[][] pages;
        private readonly int pageSize;
        private int version;

        private T[] currentPage;
        private int currentPageIndex = -1;

        public PagedList(int capacity = 0, int pageSize = DefaultPageSize)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.pageSize = pageSize;

            EnsureCapacity(capacity);
        }

        public int Capacity
        {
            get
            {
                return this.pages != null
                   ? this.pages.Length * this.pageSize
                   : 0;
            }
        }

        public int Count => this.count;
        public int PageSize => this.pageSize;

        bool ICollection<T>.IsReadOnly => false;

        public void EnsureCapacity(int value)
        {
            if (this.Capacity >= value)
            {
                return;
            }

            var pageCount = value / this.pageSize;
            if (value % this.pageSize > 0)
            {
                pageCount++;
            }

            if (pages == null)
            {
                pages = new T[pageCount][];
            }
            else
            {
                Array.Resize(ref pages, pageCount);
            }
        }

        private T[] GetPage(int pageIndex)
        {
            if (currentPageIndex == pageIndex)
            {
                return this.currentPage;
            }

            var page = this.pages[pageIndex];
            if (page == null)
            {
                page = new T[pageSize];
                this.pages[pageIndex] = page;
            }

            this.currentPage = page;
            this.currentPageIndex = pageIndex;

            return page;
        }

        private T GetItem(int index)
        {
            var pageIndex = index / this.pageSize;
            var itemIndex = index % this.pageSize;

            var page = GetPage(pageIndex);

            return page[itemIndex];
        }

        private void SetItem(int index, T value)
        {
            var pageIndex = index / this.pageSize;
            var itemIndex = index % this.pageSize;

            var page = GetPage(pageIndex);

            page[itemIndex] = value;
        }

        private void ShiftItemsRight(int index)
        {
            Debug.Assert(this.Capacity > this.count);

            var pageIndex = index / this.pageSize;

            for (int i = this.pages.Length - 1; i >= pageIndex; i--)
            {
                var page = this.pages[i];
                if (page != null)
                {
                    // Don't bother copying the last item of the last page since there
                    // isn't a next page. Note: because the capacity is greater than the
                    // count, this is OK.
                    if (i < this.pages.Length - 1)
                    {
                        var lastitem = page[page.Length - 1];
                        SetItem((i + 1) * this.pageSize, lastitem);
                    }

                    var itemIndex = i == pageIndex
                        ? index % this.pageSize
                        : 0;

                    Array.Copy(page, itemIndex, page, itemIndex + 1, page.Length - itemIndex - 1);
                }
            }
        }

        private void ShiftItemsLeft(int index)
        {
            var pageIndex = index / this.pageSize;

            for (int i = pageIndex; i < this.pages.Length; i++)
            {
                var page = this.pages[i];
                if (page != null)
                {
                    if (i > pageIndex)
                    {
                        var firstItem = page[0];
                        SetItem((i * this.pageSize) - 1, firstItem);
                    }

                    var itemIndex = i == pageIndex
                        ? index % this.pageSize
                        : 0;

                    Array.Copy(page, itemIndex + 1, page, itemIndex, page.Length - itemIndex - 1);

                    page[page.Length - 1] = default(T);
                }
            }
        }

        public void Add(T item)
        {
            if (this.count == this.Capacity)
            {
                EnsureCapacity(this.count + 1);
            }

            SetItem(this.count, item);
            this.count++;
            this.version++;
        }

        public void Clear()
        {
            if (this.count > 0)
            {
                Array.Clear(this.pages, 0, this.pages.Length);
                this.count = 0;
            }

            this.version++;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (this.pages != null)
            {
                for (int p = 0; p < this.pages.Length - 1; p++)
                {
                    var pageStart = p * this.pageSize;
                    Array.Copy(this.pages[p], 0, array, pageStart + arrayIndex, this.pageSize);
                }

                var lastPageIndex = this.pages.Length - 1;
                var lastPageStart = lastPageIndex * this.pageSize;
                Array.Copy(this.pages[lastPageIndex], 0, array, lastPageStart + arrayIndex, this.count - lastPageStart);
            }
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public int IndexOf(T item)
        {
            if (this.pages != null)
            {
                for (int p = 0; p < this.pages.Length; p++)
                {
                    var page = this.pages[p];
                    if (page != null)
                    {
                        var i = Array.IndexOf(page, item);
                        if (i >= 0)
                        {
                            return (p * this.pageSize) + i;
                        }
                    }
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index > this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (this.count == this.Capacity)
            {
                EnsureCapacity(this.count + 1);
            }

            if (index < this.count)
            {
                ShiftItemsRight(index);
            }

            SetItem(index, item);
            this.count++;
            this.version++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.count--;

            if (index < this.count)
            {
                ShiftItemsLeft(index);
            }

            SetItem(this.count, default(T));
            this.version++;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return GetItem(index);
            }

            set
            {
                if (index <0 || index >= this.count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                SetItem(index, value);
                this.version++;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => new Enumerator(this);
    }
}