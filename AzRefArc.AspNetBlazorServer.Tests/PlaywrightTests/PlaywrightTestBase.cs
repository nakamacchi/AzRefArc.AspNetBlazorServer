using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public abstract class PlaywrightTestBase : PageTest
{
    protected const string BaseUrl = "https://localhost:7268";
    
    public override BrowserNewContextOptions ContextOptions()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var videoDir = Path.Combine(baseDir, "TestResults", "videos");
        Directory.CreateDirectory(videoDir);
        
        return new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new() { Width = 1920, Height = 1080 },
            RecordVideoDir = videoDir,
            RecordVideoSize = new() { Width = 1920, Height = 1080 }
        };
    }
    
    /// <summary>
    /// テスト実行後にスクリーンショットを撮影
    /// </summary>
    /// <param name="testName">テスト名</param>
    protected async Task TakeScreenshotAsync(string testName)
    {
        try
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var screenshotDir = Path.Combine(baseDir, "TestResults", "screenshots");
            Directory.CreateDirectory(screenshotDir);
            var screenshotPath = Path.Combine(screenshotDir, $"{testName}-{DateTime.Now:yyyyMMdd-HHmmss}.png");
            Console.WriteLine($"Saving screenshot to: {screenshotPath}");
            await Page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });
            Console.WriteLine($"Screenshot saved successfully: {screenshotPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save screenshot: {ex.Message}");
        }
    }
    
    /// <summary>
    /// テスト終了時に自動でスクリーンショットを撮影
    /// </summary>
    [TestCleanup]
    public async Task TestCleanupAsync()
    {
        try
        {
            var testName = TestContext?.TestName ?? "UnknownTest";
            Console.WriteLine($"TestCleanup executing for test: {testName}");
            await TakeScreenshotAsync(testName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in TestCleanup: {ex.Message}");
        }
    }
    
    /// <summary>
    /// ページのタイトルが期待値と一致することを確認
    /// </summary>
    /// <param name="expectedTitle">期待されるタイトル</param>
    protected async Task AssertPageTitle(string expectedTitle)
    {
        await Expect(Page).ToHaveTitleAsync(expectedTitle);
    }
    
    /// <summary>
    /// 指定されたテキストが存在することを確認
    /// </summary>
    /// <param name="text">確認するテキスト</param>
    protected async Task AssertTextExists(string text)
    {
        await Expect(Page.Locator($"text={text}")).ToBeVisibleAsync();
    }
    
    /// <summary>
    /// テーブルの行数を確認
    /// </summary>
    /// <param name="selector">テーブルのセレクタ</param>
    /// <param name="expectedRowCount">期待される行数（ヘッダー行を除く）</param>
    protected async Task AssertTableRowCount(string selector, int expectedRowCount)
    {
        var rows = Page.Locator($"{selector} tbody tr");
        await Expect(rows).ToHaveCountAsync(expectedRowCount);
    }
}
