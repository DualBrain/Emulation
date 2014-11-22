using Xunit;

namespace Emulation.Core.Tests
{
    public class MemoryTests
    {
        [Fact(DisplayName ="Default page size is 4096")]
        public void DefaultPageSize()
        {
            var memory = new Memory();
            Assert.Equal(4096, memory.PageSize);
        }
    }
}
