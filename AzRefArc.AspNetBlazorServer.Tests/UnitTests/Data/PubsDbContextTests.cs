using AzRefArc.AspNetBlazorServer.Tests.Helpers;

namespace AzRefArc.AspNetBlazorServer.Tests.UnitTests.Data
{
    /// <summary>
    /// PubsDbContextの単体テスト
    /// </summary>
    [TestClass]
    public class PubsDbContextTests
    {
        private PubsDbContext? _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = TestDbContextHelper.CreateInMemoryDbContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        [TestMethod]
        public void PubsDbContext_DatabaseShouldBeCreated()
        {
            // Arrange & Act
            var created = _context!.Database.EnsureCreated();

            // Assert
            Assert.IsTrue(created);
        }

        [TestMethod]
        public async Task PubsDbContext_CanSaveChanges()
        {
            // Arrange
            _context!.Database.EnsureCreated();

            // Act
            var result = await _context.SaveChangesAsync();

            // Assert
            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void PubsDbContext_HasCorrectDatabaseProvider()
        {
            // Arrange & Act
            var providerName = _context!.Database.ProviderName;

            // Assert
            Assert.AreEqual("Microsoft.EntityFrameworkCore.InMemory", providerName);
        }
    }
}
