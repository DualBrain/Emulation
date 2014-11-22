using System;
using System.Diagnostics;

namespace Emulation.Core
{
    public class Memory
    {
        private readonly int size;
        private readonly int pageSize;
        private readonly int pageCount;

        private readonly byte[][] pages;

        public static Memory Create(int size = 4096, int pageSize = 4096)
        {
            return new Memory(size, pageSize);
        }

        private Memory(int size, int pageSize)
        {
            this.size = size;
            this.pageSize = pageSize;

            this.pageCount = size / pageSize;
            if (size % pageSize > 0)
            {
                this.pageCount += 1;
            }

            this.pages = new byte[this.pageCount][];
        }

        private void EnsurePage(int page)
        {
            if (this.pages[page] == null)
            {
                this.pages[page] = new byte[this.pageSize];
            }
        }

        private byte ReadByteCore(int address)
        {
            Debug.Assert(address >= 0 && address < this.size);

            var page = address / this.pageSize;
            var index = address % this.pageSize;

            EnsurePage(page);

            return this.pages[page][index];
        }

        private void WriteByteCore(int address, byte value)
        {
            Debug.Assert(address >= 0 && address < this.size);

            var page = address / this.pageSize;
            var index = address % this.pageSize;

            EnsurePage(page);

            this.pages[page][index] = value;
        }

        public byte ReadByte(int address)
        {
            if (address < 0 || address >= this.size)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            return ReadByteCore(address);
        }

        public void WriteByte(int address, byte value)
        {
            if (address < 0 || address >= this.size)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            WriteByteCore(address, value);
        }

        public int Size => size;
        public int PageSize => pageSize;

        public byte this[int address]
        {
            get { return ReadByte(address); }
            set { WriteByte(address, value); }
        }
    }
}
