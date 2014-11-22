namespace Emulation.Core
{
    public class Memory
    {
        private readonly int pageSize;

        public Memory(int pageSize = 4096)
        {
            this.pageSize = pageSize;
        }

        public int PageSize => pageSize;
    }
}
