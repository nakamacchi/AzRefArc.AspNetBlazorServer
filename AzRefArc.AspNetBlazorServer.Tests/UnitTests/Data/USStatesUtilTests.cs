namespace AzRefArc.AspNetBlazorServer.Tests.UnitTests.Data
{
    /// <summary>
    /// USStatesUtilの単体テスト
    /// </summary>
    public class USStatesUtilTests
    {
        [Fact]
        public void USStatesUtil_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(USStatesUtil);

            // Assert
            Assert.NotNull(type);
        }

        [Fact]
        public void GetAllStates_ReturnsNonEmptyDictionary()
        {
            // Arrange & Act
            var states = USStatesUtil.GetAllStates();

            // Assert
            Assert.NotNull(states);
            Assert.NotEmpty(states);
        }

        [Theory]
        [InlineData("CA", "California")]
        [InlineData("NY", "New York")]
        [InlineData("TX", "Texas")]
        public void GetAllStates_ContainsExpectedStates(string stateCode, string expectedName)
        {
            // Arrange
            var states = USStatesUtil.GetAllStates();

            // Act & Assert
            Assert.True(states.ContainsKey(stateCode), $"州コード '{stateCode}' が見つかりません");
            Assert.Equal(expectedName, states[stateCode]);
        }

        [Fact]
        public void GetAllStates_DoesNotContainInvalidStateCode()
        {
            // Arrange
            var states = USStatesUtil.GetAllStates();
            var invalidStateCode = "XX";

            // Act & Assert
            Assert.False(states.ContainsKey(invalidStateCode), $"無効な州コード '{invalidStateCode}' が含まれています");
        }
    }
}
