using System;
using System.Collections.Generic;

namespace Emulation.Debugger.MVVM
{
    public partial class BulkObservableCollection<T>
    {
        private class FuncComparer : IComparer<T>
        {
            private readonly Func<T, T, int> comparer;

            public FuncComparer(Func<T, T, int> comparer)
            {
                this.comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return comparer(x, y);
            }
        }
    }
}
