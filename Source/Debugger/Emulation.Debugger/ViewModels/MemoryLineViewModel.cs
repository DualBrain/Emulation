using Emulation.Core;
using Emulation.Debugger.MVVM;

namespace Emulation.Debugger.ViewModels
{
    internal class MemoryLineViewModel : ViewModel
    {
        private readonly Memory memory;
        private readonly int address;
        private readonly int hexWidth;

        public MemoryLineViewModel(Memory memory, int address, int hexWidth)
        {
            this.memory = memory;
            this.address = address;
            this.hexWidth = hexWidth;
        }

        private byte? GetValue(int address) =>
            address >= 0 && address < this.memory.Size
                ? this.memory.ReadByte(address)
                : (byte?)null;

        private char? GetChar(int address)
        {
            var value = GetValue(address);
            if (value == null)
            {
                return null;
            }

            var ch = (char)value.Value;
            return char.IsControl(ch)
                ? '.'
                : ch == (char)0xad // soft hyphens are ignore in WPF
                    ? '-'
                    : ch;
        }

        public int Address => this.address;
        public string AddressText => address.ToString("x" + this.hexWidth.ToString());

        public byte? Byte1 => GetValue(this.address);
        public byte? Byte2 => GetValue(this.address + 1);
        public byte? Byte3 => GetValue(this.address + 2);
        public byte? Byte4 => GetValue(this.address + 3);
        public byte? Byte5 => GetValue(this.address + 4);
        public byte? Byte6 => GetValue(this.address + 5);
        public byte? Byte7 => GetValue(this.address + 6);
        public byte? Byte8 => GetValue(this.address + 7);
        public byte? Byte9 => GetValue(this.address + 8);
        public byte? Byte10 => GetValue(this.address + 9);
        public byte? Byte11 => GetValue(this.address + 10);
        public byte? Byte12 => GetValue(this.address + 11);
        public byte? Byte13 => GetValue(this.address + 12);
        public byte? Byte14 => GetValue(this.address + 13);
        public byte? Byte15 => GetValue(this.address + 14);
        public byte? Byte16 => GetValue(this.address + 15);

        public char? Char1 => GetChar(this.address);
        public char? Char2 => GetChar(this.address + 1);
        public char? Char3 => GetChar(this.address + 2);
        public char? Char4 => GetChar(this.address + 3);
        public char? Char5 => GetChar(this.address + 4);
        public char? Char6 => GetChar(this.address + 5);
        public char? Char7 => GetChar(this.address + 6);
        public char? Char8 => GetChar(this.address + 7);
        public char? Char9 => GetChar(this.address + 8);
        public char? Char10 => GetChar(this.address + 9);
        public char? Char11 => GetChar(this.address + 10);
        public char? Char12 => GetChar(this.address + 11);
        public char? Char13 => GetChar(this.address + 12);
        public char? Char14 => GetChar(this.address + 13);
        public char? Char15 => GetChar(this.address + 14);
        public char? Char16 => GetChar(this.address + 15);
    }
}
