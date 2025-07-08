using AzRefArc.AspNetBlazorServer.Tests.Helpers;

namespace AzRefArc.AspNetBlazorServer.Tests.UnitTests.Data
{
    /// <summary>
    /// PubsDbContextの単体テスト
    /// </summary>
    public class PubsDbContextTests : IDisposable
    {
        private readonly PubsDbContext _context;

        public PubsDbContextTests()
        {
            _context = TestDbContextHelper.CreateInMemoryDbContext();
        }

        [Fact]
        public void PubsDbContext_DatabaseShouldBeCreated()
        {
            // Arrange & Act
            var created = _context.Database.EnsureCreated();

            // Assert
            Assert.True(created);
        }

        [Fact]
        public async Task PubsDbContext_CanSaveChanges()
        {
            // Arrange
            _context.Database.EnsureCreated();

            // Act
            var result = await _context.SaveChangesAsync();

            // Assert
            Assert.True(result >= 0);
        }

        [Fact]
        public void PubsDbContext_HasCorrectDatabaseProvider()
        {
            // Arrange & Act
            var providerName = _context.Database.ProviderName;

            // Assert
            Assert.Equal("Microsoft.EntityFrameworkCore.InMemory", providerName);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
