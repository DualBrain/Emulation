using Xunit;

namespace Emulation.Core.Tests
{
    public class BytesTests
    {
        [Fact(DisplayName = "Bytes.Reverse for ushort")]
        public void ReverseUInt16()
        {
            const ushort value = 0x0123;
            const ushort expected = 0x2301;

            Assert.Equal(expected, Bytes.Reverse(value));
        }

        [Fact(DisplayName = "Bytes.Reverse for uint")]
        public void ReverseUInt32()
        {
            const uint value = 0x01234567;
            const uint expected = 0x67452301;

            Assert.Equal(expected, Bytes.Reverse(value));
        }

        [Fact(DisplayName = "Bytes.Reverse for ulong")]
        public void ReverseUInt64()
        {
            const ulong value = 0x0123456789abcdef;
            const ulong expected = 0xefcdab8967452301;

            Assert.Equal(expected, Bytes.Reverse(value));
        }
    }
}
