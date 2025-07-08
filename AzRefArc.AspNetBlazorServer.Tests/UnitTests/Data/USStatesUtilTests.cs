namespace AzRefArc.AspNetBlazorServer.Tests.UnitTests.Data
{
    /// <summary>
    /// USStatesUtilの単体テスト
    /// </summary>
    [TestClass]
    public class USStatesUtilTests
    {
        [TestMethod]
        public void USStatesUtil_ShouldExist()
        {
            // Arrange & Act
            var type = typeof(USStatesUtil);

            // Assert
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void GetAllStates_ReturnsNonEmptyDictionary()
        {
            // Arrange & Act
            var states = USStatesUtil.GetAllStates();

            // Assert
            Assert.IsNotNull(states);
            Assert.IsTrue(states.Count > 0);
        }

        [TestMethod]
        [DataRow("CA", "California")]
        [DataRow("NY", "New York")]
        [DataRow("TX", "Texas")]
        public void GetAllStates_ContainsExpectedStates(string stateCode, string expectedName)
        {
            // Arrange
            var states = USStatesUtil.GetAllStates();

            // Act & Assert
            Assert.IsTrue(states.ContainsKey(stateCode), $"州コード '{stateCode}' が見つかりません");
            Assert.AreEqual(expectedName, states[stateCode]);
        }

        [TestMethod]
        public void GetAllStates_DoesNotContainInvalidStateCode()
        {
            // Arrange
            var states = USStatesUtil.GetAllStates();
            var invalidStateCode = "XX";

            // Act & Assert
            Assert.IsFalse(states.ContainsKey(invalidStateCode), $"無効な州コード '{invalidStateCode}' が含まれています");
        }
    }
}
