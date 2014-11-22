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

        private byte[] currentPage;
        private int currentPageStart;
        private int nextPageStart;

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

        private void SelectPage(int address)
        {
            var pageIndex = address / this.pageSize;

            if (this.pages[pageIndex] != null)
            {
                this.currentPage = this.pages[pageIndex];
            }
            else
            {
                this.currentPage = new byte[this.pageSize];
                this.pages[pageIndex] = this.currentPage;
            }

            this.currentPage = this.pages[pageIndex];
            this.currentPageStart = pageIndex * this.pageSize;
            this.nextPageStart = this.currentPageStart + this.pageSize;
        }

        private byte ReadByteCore(int address)
        {
            Debug.Assert(address >= 0 && address < this.size);

            if (address < this.currentPageStart || address >= this.nextPageStart)
            {
                SelectPage(address);
            }

            return this.currentPage[address - this.currentPageStart];
        }

        private void WriteByteCore(int address, byte value)
        {
            Debug.Assert(address >= 0 && address < this.size);

            if (address < this.currentPageStart || address >= this.nextPageStart)
            {
                SelectPage(address);
            }

            this.currentPage[address - this.currentPageStart] = value;
        }

        public byte ReadByte(int address)
        {
            if (address < 0 || address > this.size - 1)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            return ReadByteCore(address);
        }

        public void WriteByte(int address, byte value)
        {
            if (address < 0 || address > this.size - 1)
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
