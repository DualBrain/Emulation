namespace Emulation.Core.Tests
{
    public static class Helpers
    {
        public static byte IndexToByte(int i)
        {
            return (byte)(i % 256);
        }

        public static ushort IndexToUInt16(int i)
        {
            var b1 = IndexToByte(i);
            var b2 = IndexToByte(i + 1);

            return (ushort)((b1 << 8) | b2);
        }

        public static uint IndexToUInt32(int i)
        {
            var b1 = IndexToByte(i);
            var b2 = IndexToByte(i + 1);
            var b3 = IndexToByte(i + 2);
            var b4 = IndexToByte(i + 3);

            return (uint)((b1 << 24) | (b2 << 16) | (b3 << 8) | b4);
        }
    }
}
