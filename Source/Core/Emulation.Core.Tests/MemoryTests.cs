using System;
using System.IO;
using Xunit;

namespace Emulation.Core.Tests
{
    public class MemoryTests
    {
        [Fact(DisplayName = "Default page size is 4096")]
        public void DefaultPageSize()
        {
            var memory = Memory.CreateEmpty();
            Assert.Equal(4096, memory.PageSize);
        }

        [Fact(DisplayName = "Default size is 4096")]
        public void DefaultSize()
        {
            var memory = Memory.CreateEmpty();
            Assert.Equal(4096, memory.Size);
        }

        [Fact(DisplayName = "Write and read bytes into default memory using indexer")]
        public void WriteAndReadBytes_Indexer()
        {
            var memory = Memory.CreateEmpty();

            for (int i = 0; i < memory.Size; i++)
            {
                memory[i] = Helpers.IndexToByte(i);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(Helpers.IndexToByte(i), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory using indexer")]
        public void WriteAndReadBytes_64k_Indexer()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory[i] = Helpers.IndexToByte(i);
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(Helpers.IndexToByte(i), memory[i]);
            }
        }

        [Fact(DisplayName = "Write and read bytes into 64k memory")]
        public void WriteAndReadBytes_64k()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory.WriteByte(i, Helpers.IndexToByte(i));
            }

            for (int i = 0; i < memory.Size; i++)
            {
                Assert.Equal(Helpers.IndexToByte(i), memory.ReadByte(i));
            }
        }

        [Fact(DisplayName = "Reading and writing bytes out of range throws")]
        public void ReadAndWriteBytesOutOfRange()
        {
            var memory = Memory.CreateEmpty();

            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadByte(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteByte(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadByte(memory.Size));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.WriteByte(memory.Size, 0));
        }

        [Fact(DisplayName = "Write and read ushorts into 64k memory")]
        public void WriteAndReadUInt16s_64k()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i += 2)
            {
                memory.WriteUInt16(i, Helpers.IndexToUInt16(i));
            }

            for (int i = 0; i < memory.Size; i += 2)
            {
                Assert.Equal(Helpers.IndexToUInt16(i), memory.ReadUInt16(i));
            }
        }

        [Fact(DisplayName = "Write and read ushorts into 64k memory starting at address 1")]
        public void WriteAndReadUInt16s_64k_Offset()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 1; i < memory.Size - 1; i += 2)
            {
                memory.WriteUInt16(i, Helpers.IndexToUInt16(i));
            }

            for (int i = 1; i < memory.Size - 1; i += 2)
            {
                Assert.Equal(Helpers.IndexToUInt16(i), memory.ReadUInt16(i));
            }
        }

        [Fact(DisplayName = "Reading and writing ushorts out of range throws")]
        public void ReadAndWriteUInt16sOutOfRange()
        {
            var memory = Memory.CreateEmpty();

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
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i += 4)
            {
                memory.WriteUInt32(i, Helpers.IndexToUInt32(i));
            }

            for (int i = 0; i < memory.Size; i += 4)
            {
                Assert.Equal(Helpers.IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 1")]
        public void WriteAndReadUInt32s_64k_Offset1()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 1; i < memory.Size - 3; i += 4)
            {
                memory.WriteUInt32(i, Helpers.IndexToUInt32(i));
            }

            for (int i = 1; i < memory.Size - 3; i += 4)
            {
                Assert.Equal(Helpers.IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 2")]
        public void WriteAndReadUInt32s_64k_Offset2()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 2; i < memory.Size - 2; i += 4)
            {
                memory.WriteUInt32(i, Helpers.IndexToUInt32(i));
            }

            for (int i = 2; i < memory.Size - 2; i += 4)
            {
                Assert.Equal(Helpers.IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Write and read uints into 64k memory starting at address 3")]
        public void WriteAndReadUInt32s_64k_Offset3()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 3; i < memory.Size - 1; i += 4)
            {
                memory.WriteUInt32(i, Helpers.IndexToUInt32(i));
            }

            for (int i = 3; i < memory.Size - 1; i += 4)
            {
                Assert.Equal(Helpers.IndexToUInt32(i), memory.ReadUInt32(i));
            }
        }

        [Fact(DisplayName = "Reading and writing uints out of range throws")]
        public void ReadAndWriteUInt32sOutOfRange()
        {
            var memory = Memory.CreateEmpty();

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

        [Fact(DisplayName = "Create memory from stream")]
        public void CreateFromStream()
        {
            const int length = (64 * 1024) + 42;

            using (var stream = new MemoryStream(length))
            {
                for (int i = 0; i < length; i++)
                {
                    stream.WriteByte(Helpers.IndexToByte(i));
                }

                stream.Seek(0, SeekOrigin.Begin);

                var memory = Memory.CreateFromStream(stream);
                Assert.Equal(length, memory.Size);

                for (int i = 0; i < length; i++)
                {
                    Assert.Equal(Helpers.IndexToByte(i), memory.ReadByte(i));
                }
            }
        }

        [Fact(DisplayName = "Call ReadBytes()")]
        public void ReadBytes()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            for (int i = 0; i < memory.Size; i++)
            {
                memory.WriteByte(i, Helpers.IndexToByte(i));
            }

            byte[] buffer = new byte[8192];
            memory.ReadBytes(buffer, 256, 256, buffer.Length - 256);

            for (int i = 256; i < 8192; i++)
            {
                Assert.Equal(Helpers.IndexToByte(i), buffer[i]);
            }
        }

        [Fact(DisplayName = "Call ReadBytes() with bad arguments")]
        public void ReadBytesWithBadArguments()
        {
            var memory = Memory.CreateEmpty(size: 64 * 1024);

            Assert.Throws<ArgumentNullException>(() => memory.ReadBytes(null, 0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], -1, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 1024, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, (64 * 1024) + 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, (64 * 1024), 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, 0, (64 * 1024) + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => memory.ReadBytes(new byte[1024], 0, 0, (64 * 1024) + 1));
            Assert.Throws<ArgumentException>(() => memory.ReadBytes(new byte[1024], 0, 0, 1025));
        }
    }
}
