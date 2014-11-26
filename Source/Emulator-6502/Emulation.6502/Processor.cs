using Emulation.Core;

namespace Emulation_6502
{
    public class Processor
    {
        private readonly Memory memory;

        // registers
        private ushort pc;
        private byte sp;
        private byte accumulator;
        private byte x;
        private byte y;
        private byte status;

        public Processor()
        {
            this.memory = Memory.Create(64 * 1024);
        }

        public Memory Memory => this.memory;

        public ushort PC => this.pc;
        public byte SP => this.sp;
        public byte Accumulator => this.accumulator;
        public byte X => this.x;
        public byte Y => this.y;
        public byte Status => this.status;
    }
}
