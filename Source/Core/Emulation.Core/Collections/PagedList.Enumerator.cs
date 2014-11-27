using System;
using System.Collections;
using System.Collections.Generic;

namespace Emulation.Core.Collections
{
    public partial class PagedList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly PagedList<T> list;
            private readonly int version;
            private int index;
            private T current;

            internal Enumerator(PagedList<T> list)
            {
                this.list = list;
                this.version = list.version;
                this.index = 0;
                this.current = default(T);
            }

            public T Current => this.current;

            object IEnumerator.Current
            {
                get
                {
                    if (this.index == 0 || this.index == this.list.count + 1)
                    {
                        throw new InvalidOperationException();
                    }

                    return this.Current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                var list = this.list;
                if (this.version == list.version && this.index < list.count)
                {
                    this.current = list.GetItem(this.index);
                    this.index++;
                    return true;
                }

                if (this.version != list.version)
                {
                    throw new InvalidOperationException();
                }

                this.index = list.count + 1;
                this.current = default(T);
                return false;
            }

            public void Reset()
            {
                if (this.version != this.list.version)
                {
                    throw new InvalidOperationException();
                }

                this.index = 0;
                this.current = default(T);
            }
        }
    }
}
