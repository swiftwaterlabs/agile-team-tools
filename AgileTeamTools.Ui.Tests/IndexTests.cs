using AgileTeamTools.Ui.Tests.TestUtility;
using Xunit;

namespace AgileTeamTools.Ui.Tests
{
    public class IndexTests
    {
        [Fact]
        public void Page_RendersCorrectly()
        {
            // Given
            var builder = TestBuilder.Create();

            // When
            var index = builder.GetComponent<Pages.Index>();

            // Then
            Assert.NotNull(index);
            Assert.NotNull(index.Instance);
            Assert.NotNull(index.Markup);
        }
    }
}
