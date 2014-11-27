using Emulation.Core.Collections;
using Xunit;

namespace Emulation.Core.Tests.Collections
{
    public class PagedListTests
    {
        [Fact(DisplayName = "default page size is 16")]
        public void DefaultPageSize()
        {
            var list1 = new PagedList<int>();
            Assert.Equal(16, list1.PageSize);

            var list2 = new PagedList<int>(4);
            Assert.Equal(16, list2.PageSize);

            var list3 = new PagedList<int>(256);
            Assert.Equal(16, list3.PageSize);
        }

        [Fact(DisplayName = "add 4k items and clear")]
        public void AddItemsAndClear()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(i, list[i]);
            }

            list.Clear();

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 64k items and clear with page size of 37")]
        public void AddItemsAndClear_PageSize37()
        {
            const int length = 64 * 1024;

            var list = new PagedList<int>(pageSize: 37);

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(i, list[i]);
            }

            list.Clear();

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 64k items and clear with page size of 4096")]
        public void AddItemsAndClear_PageSize4096()
        {
            const int length = 64 * 1024;

            var list = new PagedList<int>(pageSize: 4096);

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(i, list[i]);
            }

            list.Clear();

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "insert 4k items at front and clear")]
        public void InsertItemsAndClear()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Insert(0, i);
            }

            Assert.Equal(length, list.Count);

            for (int i = 0, j = length - 1; i < length; i++, j--)
            {
                Assert.Equal(j, list[i]);
            }

            list.Clear();

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 4k items and remove all from front")]
        public void AddItemsAndRemoveFromFront()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(i, list[0]);
                list.RemoveAt(0);
            }

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 4k items and remove all from back")]
        public void AddItemsAndRemoveFromBack()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = length - 1; i >= 0; i--)
            {
                Assert.Equal(i, list[i]);
                list.RemoveAt(i);
                Assert.Equal(i, list.Count);
            }

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 4k items and remove all")]
        public void AddItemsAndRemove()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            for (int i = length - 1; i >= 0; i--)
            {
                Assert.Equal(i, list[i]);
                Assert.True(list.Remove(i));
                Assert.Equal(i, list.Count);
            }

            Assert.Equal(0, list.Count);
        }

        [Fact(DisplayName = "add 4k items and verify Contains")]
        public void AddItemsAndVerifyContains()
        {
            const int length = 4 * 1024;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
                Assert.True(list.Contains(i));
            }

            Assert.Equal(length, list.Count);
        }

        [Fact(DisplayName = "add 4k + 42 items and copy to array")]
        public void AddItemsAndCopyToArray()
        {
            const int length = (4 * 1024) + 42;

            var list = new PagedList<int>();

            for (int i = 0; i < length; i++)
            {
                list.Add(i);
            }

            Assert.Equal(length, list.Count);

            var array = new int[length];
            list.CopyTo(array);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(i, array[i]);
            }
        }
    }
}
