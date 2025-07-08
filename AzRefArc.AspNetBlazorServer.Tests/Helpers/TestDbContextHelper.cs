using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetBlazorServer.Tests.Helpers
{
    /// <summary>
    /// テスト用のデータベースコンテキストヘルパー
    /// </summary>
    public static class TestDbContextHelper
    {
        /// <summary>
        /// インメモリデータベースを使用したPubsDbContextを作成
        /// </summary>
        /// <param name="databaseName">データベース名（テストごとに一意にする）</param>
        /// <returns>テスト用のPubsDbContext</returns>
        public static PubsDbContext CreateInMemoryDbContext(string? databaseName = null)
        {
            databaseName ??= Guid.NewGuid().ToString();
            
            var options = new DbContextOptionsBuilder<PubsDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new PubsDbContext(options);
        }

        /// <summary>
        /// テスト用のDataProtectionKeyDbContextを作成
        /// </summary>
        /// <param name="databaseName">データベース名（テストごとに一意にする）</param>
        /// <returns>テスト用のDataProtectionKeyDbContext</returns>
        public static DataProtectionKeyDbContext CreateInMemoryDataProtectionContext(string? databaseName = null)
        {
            databaseName ??= Guid.NewGuid().ToString();
            
            var options = new DbContextOptionsBuilder<DataProtectionKeyDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new DataProtectionKeyDbContext(options);
        }
    }
}
