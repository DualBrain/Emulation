using Emulation.Core;

namespace Emulation_6502
{
    public class Processor
    {
        private readonly Memory memory;

        public Processor()
        {
            this.memory = Memory.Create(64 * 1024);
        }

        public Memory Memory => this.memory;
    }
}
