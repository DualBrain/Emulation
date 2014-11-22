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

        private static uint IndexToUInt32(int i)
        {
            var b1 = IndexToByte(i);
            var b2 = IndexToByte(i + 1);
            var b3 = IndexToByte(i + 2);
            var b4 = IndexToByte(i + 3);

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
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

        [Fact(DisplayName = "Write and read uints into 64k memory")]
        public void WriteAndReadUInt32s_64k()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i += 4)
            {
                memory.WriteUInt32(i, IndexToUInt32(i));
            }

            for (int i = 0; i < memory.Size; i += 4)
            {
                Assert.Equal(IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 1")]
        public void WriteAndReadUInt32s_64k_Offset1()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 1; i < memory.Size - 3; i += 4)
            {
                memory.WriteUInt32(i, IndexToUInt32(i));
            }

            for (int i = 1; i < memory.Size - 3; i += 4)
            {
                Assert.Equal(IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 2")]
        public void WriteAndReadUInt32s_64k_Offset2()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 2; i < memory.Size - 2; i += 4)
            {
                memory.WriteUInt32(i, IndexToUInt32(i));
            }

            for (int i = 2; i < memory.Size - 2; i += 4)
            {
                Assert.Equal(IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 3")]
        public void WriteAndReadUInt32s_64k_Offset3()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 3; i < memory.Size - 1; i += 4)
            {
                memory.WriteUInt32(i, IndexToUInt32(i));
            }

            for (int i = 3; i < memory.Size - 1; i += 4)
            {
                Assert.Equal(IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Reading and writing uints out of range throws")]
        public void ReadAndWriteUInt32sOutOfRange()
        {
            var memory = Memory.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt32(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt32(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt32(memory.Size));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt32(memory.Size - 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt32(memory.Size - 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadUInt32(memory.Size - 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt32(memory.Size, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt32(memory.Size - 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt32(memory.Size - 2, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteUInt32(memory.Size - 3, 0));
        }
    }
}
