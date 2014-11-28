namespace Emulation.Core
{
    public static class Bytes
    {
        public static ushort Reverse(ushort value)
        {
            return (ushort)((value & 0x00ffu) << 8 | (value & 0xff00u) >> 8);
        }

        public static uint Reverse(uint value)
        {
            return (value & 0x000000ffu) << 24
                 | (value & 0x0000ff00u) << 8
                 | (value & 0x00ff0000u) >> 8
                 | (value & 0xff000000u) >> 24;
        }

        public static ulong Reverse(ulong value)
        {
            return (value & 0x00000000000000fful) << 56
                 | (value & 0x000000000000ff00ul) << 40
                 | (value & 0x0000000000ff0000ul) << 24
                 | (value & 0x00000000ff000000ul) << 8
                 | (value & 0x000000ff00000000ul) >> 8
                 | (value & 0x0000ff0000000000ul) >> 24
                 | (value & 0x00ff000000000000ul) >> 40
                 | (value & 0xff00000000000000ul) >> 56;
        }
    }
}
