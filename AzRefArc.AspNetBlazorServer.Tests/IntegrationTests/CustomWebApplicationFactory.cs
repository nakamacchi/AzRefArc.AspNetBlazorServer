using Microsoft.Extensions.Logging;

namespace AzRefArc.AspNetBlazorServer.Tests.IntegrationTests
{
    /// <summary>
    /// 統合テスト用のカスタムWebApplicationFactory
    /// </summary>
    /// <typeparam name="TProgram">プログラムクラス</typeparam>
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 本番用のDbContextを削除
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PubsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var factoryDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbContextFactory<PubsDbContext>));
                if (factoryDescriptor != null)
                {
                    services.Remove(factoryDescriptor);
                }

                // テスト用のインメモリデータベースを追加
                services.AddDbContextFactory<PubsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // データ保護用のコンテキストもテスト用に変更
                var dataProtectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DataProtectionKeyDbContext>));
                if (dataProtectionDescriptor != null)
                {
                    services.Remove(dataProtectionDescriptor);
                }

                services.AddDbContext<DataProtectionKeyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDataProtectionDatabase");
                });

                // ロギングの設定
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Warning);
                });
            });

            builder.UseEnvironment("Testing");
        }
    }
}
