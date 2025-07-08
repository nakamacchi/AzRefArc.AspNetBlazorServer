using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public abstract class PlaywrightTestBase : PageTest
{
    protected const string BaseUrl = "https://localhost:7268";
    
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true,
            ViewportSize = new() { Width = 1920, Height = 1080 }
        };
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
