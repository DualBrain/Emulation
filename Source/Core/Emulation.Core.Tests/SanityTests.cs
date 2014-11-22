using Xunit;

namespace Emulation.Core.Tests
{
    public class SanityTests
    {
        [Fact(DisplayName = "xUnit is working correctly.")]
        public void XunitIsWorking()
        {
            Assert.True(true);
            Assert.False(false);
        }
    }
}
