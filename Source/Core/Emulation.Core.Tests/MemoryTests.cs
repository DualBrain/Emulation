using System;
using Xunit;

namespace Emulation.Core.Tests
{
    public class MemoryTests
    {
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
                memory[i] = (byte)(i % 256);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal((byte)(i % 256), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory using indexer")]
        public void WriteAndReadBytes_64k_Indexer()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory[i] = (byte)(i % 256);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal((byte)(i % 256), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory")]
        public void WriteAndReadBytes_64k()
        {
            var memory = Memory.Create(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory.WriteByte(i, (byte)(i % 256));
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal((byte)(i % 256), memory.ReadByte(i));
            }
        }

        [Fact(DisplayName = "Reading and writing bytes out of range throws")]
        public void ReadAndWriteBytesOutOfRange()
        {
            var memory = Memory.Create();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadByte(-1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteByte(-1, 0);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.ReadByte(memory.Size);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                memory.WriteByte(memory.Size, 0);
            });
        }
    }
}
