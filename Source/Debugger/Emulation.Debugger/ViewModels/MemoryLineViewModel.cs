using System;
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
    }
}
