﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

        public static Memory CreateEmpty(int size = 4096, int pageSize = 4096)
        {
            return new Memory(size, pageSize);
        }

        public static Memory CreateFromStream(Stream stream, int pageSize = 4096)
        {
            return new Memory(stream, pageSize);
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

        private Memory(Stream stream, int pageSize)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var pages = new List<byte[]>();
            var size = 0;

            while (true)
            {
                // Check to see if we've reached the end of the stream.
                var nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    break;
                }

                var page = new byte[pageSize];

                page[0] = (byte)nextByte;
                size++;

                pages.Add(page);

                int read = stream.Read(page, 1, pageSize - 1);

                size += read;
            }

            this.size = size;
            this.pageSize = pageSize;
            this.pageCount = pages.Count;
            this.pages = pages.ToArray();
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

        public ushort ReadUInt16(int address)
        {
            if (address < 0 || address > this.size - 2)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            // We take a faster path if the entire value can be read from the current page
            if (address >= this.currentPageStart && address < this.nextPageStart - 2)
            {
                var page = this.currentPage;
                var index = address - this.currentPageStart;

                return (ushort)((page[index] << 8) | page[index + 1]);
            }
            else
            {
                var b1 = ReadByteCore(address);
                var b2 = ReadByteCore(address + 1);

                return (ushort)((b1 << 8) | b2);
            }
        }

        public uint ReadUInt32(int address)
        {
            if (address < 0 || address > this.size - 4)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            // We take a faster path if the entire value can be read from the current page
            if (address >= this.currentPageStart && address < this.nextPageStart - 4)
            {
                var page = this.currentPage;
                var index = address - this.currentPageStart;

                return (uint)((page[index] << 24) | (page[index + 1] << 16) | (page[index + 2] << 8) | page[index + 3]);
            }
            else
            {
                var b1 = ReadByteCore(address);
                var b2 = ReadByteCore(address + 1);
                var b3 = ReadByteCore(address + 2);
                var b4 = ReadByteCore(address + 3);

                return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
            }
        }

        public void WriteByte(int address, byte value)
        {
            if (address < 0 || address > this.size - 1)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            WriteByteCore(address, value);
        }

        public void WriteUInt16(int address, ushort value)
        {
            if (address < 0 || address > this.size - 2)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            var b1 = (byte)(value >> 8);
            var b2 = (byte)(value & 0xff);

            // We take a faster path if the entire value can be written to the current page
            if (address >= this.currentPageStart && address < this.nextPageStart - 2)
            {
                var page = this.currentPage;
                var index = address - this.currentPageStart;

                page[index] = b1;
                page[index + 1] = b2;
            }
            else
            {
                WriteByteCore(address, b1);
                WriteByteCore(address + 1, b2);
            }
        }

        public void WriteUInt32(int address, uint value)
        {
            if (address < 0 || address > this.size - 4)
            {
                throw new ArgumentOutOfRangeException("address");
            }

            var b1 = (byte)(value >> 24);
            var b2 = (byte)(value >> 16);
            var b3 = (byte)(value >> 8);
            var b4 = (byte)(value & 0xff);

            // We take a faster path if the entire value can be written to the current page
            if (address >= this.currentPageStart && address < this.nextPageStart - 4)
            {
                var page = this.currentPage;
                var index = address - this.currentPageStart;

                page[index] = b1;
                page[index + 1] = b2;
                page[index + 2] = b3;
                page[index + 3] = b4;
            }
            else
            {
                WriteByteCore(address, b1);
                WriteByteCore(address + 1, b2);
                WriteByteCore(address + 2, b3);
                WriteByteCore(address + 3, b4);
            }
        }

        /// <summary>
        /// Read bytes from the specified <paramref name="address"/> and copy them into
        /// the passed <paramref name="buffer"/> at the given <paramref name="index"/>.
        /// The number of bytes to be read is specified by <paramref name="length"/>.
        /// </summary>
        /// <param name="buffer">The byte array to copy the read bytes into.</param>
        /// <param name="index">The index into <paramref name="buffer"/> to copy the read bytes.</param>
        /// <param name="address">The address to read the bytes from.</param>
        /// <param name="length">The number of bytes to read.</param>
        public void ReadBytes(byte[] buffer, int index, int address, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (index < 0 || index >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (address < 0 || address > this.size - length)
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }

            if (length < 0 || length > this.size)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (buffer.Length - index < length)
            {
                throw new ArgumentException(
                    "There is not enough room in \{nameof(buffer)} to copy \{length} bytes to index \{index}.",
                    paramName: nameof(buffer));
            }

            while (length > 0)
            {
                SelectPage(address);

                var amountToRead = Math.Min(length, this.nextPageStart - address);
                Array.Copy(this.currentPage, address - this.currentPageStart, buffer, index, amountToRead);

                length -= amountToRead;
                address += amountToRead;
                index += amountToRead;
            }
        }

        /// <summary>
        /// Write bytes from the specified <paramref name="buffer"/> into memory at the
        /// specified <paramref name="address"/>. The number of bytes to be written is
        /// specified by <paramref name="length"/>.
        /// </summary>
        /// <param name="buffer">The byte array to copy bytes from.</param>
        /// <param name="index">The index into <paramref name="buffer"/> to use.</param>
        /// <param name="address">The address to write the bytes to.</param>
        /// <param name="length">The number of bytes to write.</param>
        public void WriteBytes(byte[] buffer, int index, int address, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (index < 0 || (index == buffer.Length && length == 0) || index > buffer.Length - length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (address < 0 || address > this.size - length)
            {
                throw new ArgumentOutOfRangeException(nameof(address));
            }

            if (length < 0 || length > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            while (length > 0)
            {
                SelectPage(address);

                var amountToWrite = Math.Min(length, this.nextPageStart - address);
                Array.Copy(buffer, index, this.currentPage, address - this.currentPageStart, amountToWrite);

                length -= amountToWrite;
                address += amountToWrite;
                index += amountToWrite;
            }
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
