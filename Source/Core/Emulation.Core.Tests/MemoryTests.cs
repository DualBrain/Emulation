using System;
using Xunit;

namespace Emulation.Core.Tests
{
    public class MemoryTests
    {
        private static byte IndexToByte(int i)
        {
            return (byte)(i % 256);
        }

        private static ushort IndexToUInt16(int i)
        {
            var b1 = IndexToByte(i);
            var b2 = IndexToByte(i + 1);

            return (ushort)((b1 << 8) | b2);
        }

        [Fact(DisplayName = "Default page size is 4096")]
        public void DefaultPageSize()
        {
            var memory = Memory.Create();
            Assert.Equal(4096, memory.PageSize);
        }

        [Fact(DisplayName = "Default size is 4096")]
        public void DefaultSize()
        {
            var memory = Memory.Create();
            Assert.Equal(4096, memory.Size);
        }

        [Fact(DisplayName = "Write and read bytes into default memory using indexer")]
        public void WriteAndReadBytes_Indexer()
        {
            var memory = Memory.Create();

            for (int i = 0; i < memory.Size; i++)
            {
                memory[i] = IndexToByte(i);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(IndexToByte(i), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory using indexer")]
        public void WriteAndReadBytes_64k_Indexer()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory[i] = IndexToByte(i);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(IndexToByte(i), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory")]
        public void WriteAndReadBytes_64k()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory.WriteByte(i, IndexToByte(i));
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(IndexToByte(i), memory.ReadByte(i));
            }
        }

        [Fact(DisplayName = "Reading and writing bytes out of range throws")]
        public void ReadAndWriteBytesOutOfRange()
        {
            var memory = Memory.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadByte(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteByte(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadByte(memory.Size));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteByte(memory.Size, 0));
        }

        [Fact(DisplayName = "Write and read ushorts into 64k memory")]
        public void WriteAndReadUInt16s_64k()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i += 2)
            {
                memory.WriteUInt16(i, IndexToUInt16(i));
            }

            for (int i = 0; i < memory.Size; i += 2)
            {
                Assert.Equal(IndexToUInt16(i), memory.ReadUInt16(i));
            }
        }

        [Fact(DisplayName = "Write and read ushorts into 64k memory starting at address 1")]
        public void WriteAndReadUInt16s_64k_Offset()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 1; i < memory.Size - 1; i += 2)
            {
                memory.WriteUInt16(i, IndexToUInt16(i));
            }

            for (int i = 1; i < memory.Size - 1; i += 2)
            {
                Assert.Equal(IndexToUInt16(i), memory.ReadUInt16(i));
            }
        }

        [Fact(DisplayName = "Reading and writing ushorts out of range throws")]
        public void ReadAndWriteUInt16sOutOfRange()
        {
            var memory = Memory.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt16(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt16(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt16(memory.Size));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt16(memory.Size - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt16(memory.Size, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt16(memory.Size - 1, 0));
        }
    }
}
