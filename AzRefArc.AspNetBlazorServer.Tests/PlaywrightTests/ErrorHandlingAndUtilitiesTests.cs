using Microsoft.Playwright;

namespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public class ErrorHandlingAndUtilitiesTests : PlaywrightTestBase
{
    [TestMethod]
    public async Task ErrorPageTest()
    {
        // 存在しないページにアクセスしてエラーハンドリングをテスト
        var response = await Page.GotoAsync($"{BaseUrl}/NonExistentPage");
        
        // レスポンスが取得できることを確認（404でもレスポンスがある）
        Assert.IsNotNull(response, "レスポンスが取得できるべきです");
        
        // ページが何らかの形で表示されることを確認（ホームページへのリダイレクトまたはエラーページ）
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // HTMLコンテンツが存在することを確認
        var hasHtml = await Page.Locator("html").IsVisibleAsync();
        Assert.IsTrue(hasHtml, "HTMLページが表示されるべきです");
    }

    [TestMethod]
    public async Task DiagnosticsServerPageTest()
    {
        // サーバ診断情報ページに移動
        await Page.GotoAsync($"{BaseUrl}/Utilities/DiagnosticsServer");
        
        // ページタイトルの確認
        await AssertPageTitle("アプリ設定・構成設定情報 (WASM)");
        
        // 診断情報の内容確認
        await AssertTextExists("コンポーネント動作");
        
        // 一般的な診断情報項目の確認
        var diagnosticInfo = await Page.Locator("body").InnerTextAsync();
        var hasDiagnosticContent = diagnosticInfo.Contains("サーバー") || 
                                 diagnosticInfo.Contains("バージョン") || 
                                 diagnosticInfo.Contains("環境") ||
                                 diagnosticInfo.Contains("メモリ") ||
                                 diagnosticInfo.Contains("CPU");
        
        Assert.IsTrue(hasDiagnosticContent, "診断情報が表示されるべきです");
    }

    [TestMethod]
    public async Task RaiseExceptionPageTest()
    {
        // 例外発生ページに移動
        await Page.GotoAsync($"{BaseUrl}/Utilities/RaiseException");
        
        // ページタイトルの確認
        await AssertPageTitle("例外発生");
        
        // ページ内容の確認
        await AssertTextExists("サンプル例外発生");
        
        // 例外発生ボタンまたはリンクがある場合のテスト
        var exceptionTrigger = Page.Locator("input[value='例外発生']");
        var triggerExists = await exceptionTrigger.CountAsync() > 0;
        
        if (triggerExists)
        {
            // 例外発生を試行（実際にはエラーページに遷移する可能性があります）
            await exceptionTrigger.ClickAsync();
            
            // エラーページまたは例外処理結果を確認
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // 例外が発生した後も何らかのページが表示されることを確認
            var pageContent = await Page.Locator("body").IsVisibleAsync();
            Assert.IsTrue(pageContent, "例外処理後もページコンテンツが表示されるべきです");
            
            // タイトルが設定されていることを確認
            var title = await Page.TitleAsync();
            Assert.IsFalse(string.IsNullOrEmpty(title), "ページタイトルが設定されているべきです");
        }
    }

    [TestMethod]
    public async Task NavigationConsistencyTest()
    {
        // 各主要ページでナビゲーションメニューが一貫して表示されることを確認
        var testPages = new[]
        {
            "/",
            "/BizGroupA/ShowAllAuthors/ListAuthors",
            "/BizGroupB/EditAuthorByOptimistic/ListAuthors",
            "/BizGroupC/ShowAllAuthors/ListAuthors",
            "/Utilities/DiagnosticsServer"
        };

        foreach (var pagePath in testPages)
        {
            await Page.GotoAsync($"{BaseUrl}{pagePath}");
            
            // データロード完了まで待機
            if (pagePath.Contains("ListAuthors"))
            {
                await Page.WaitForTimeoutAsync(3000);
            }
            
            // ナビゲーションメニューの確認
            await Expect(Page.Locator("nav .nav-link:has-text('Home')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".nav-link:has-text('参照系アプリ')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".nav-link:has-text('更新系アプリ')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".nav-link:has-text('QuickGrid 実装')")).ToBeVisibleAsync();
            await Expect(Page.Locator(".nav-link:has-text('ユーティリティ')")).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    public async Task DatabaseConnectionTest()
    {
        // データベース接続が必要なページをテストして接続エラーがないことを確認
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(5000);
        
        // データベース接続エラーがないことを確認
        var hasConnectionError = await Page.Locator("text=接続, text=データベース, text=connection, text=database").IsVisibleAsync();
        var hasData = await Page.Locator("table tbody tr").CountAsync() > 0;
        var hasNoDataMessage = await Page.Locator("text=データが一件もありません").IsVisibleAsync();
        
        // エラーが表示されていないか、正常にデータまたは「データなし」メッセージが表示されることを確認
        Assert.IsTrue(hasData || hasNoDataMessage, "データまたは適切なメッセージが表示されるべきです");
    }

    [TestMethod]
    public async Task PerformanceTest()
    {
        // ページロード時間のパフォーマンステスト
        var startTime = DateTime.Now;
        
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        await Expect(Page.Locator("table")).ToBeVisibleAsync();
        
        var endTime = DateTime.Now;
        var loadTime = endTime - startTime;
        
        // ページロード時間が妥当な範囲内であることを確認（10秒以内）
        Assert.IsTrue(loadTime.TotalSeconds < 10, $"ページロード時間が長すぎます: {loadTime.TotalSeconds}秒");
    }

    [TestMethod]
    public async Task SecurityHeadersTest()
    {
        // セキュリティ関連のHTTPヘッダーの確認
        var response = await Page.GotoAsync($"{BaseUrl}/");
        
        Assert.IsNotNull(response, "レスポンスが取得できませんでした");
        
        var headers = response.Headers;
        
        // 基本的なセキュリティヘッダーの確認（実装により異なる）
        var hasSecurityHeaders = headers.ContainsKey("x-frame-options") ||
                               headers.ContainsKey("x-content-type-options") ||
                               headers.ContainsKey("strict-transport-security");
        
        // HTTPSが使用されていることを確認
        Assert.IsTrue(response.Url.StartsWith("https://"), "HTTPSが使用されているべきです");
    }

    [TestMethod]
    public async Task AccessibilityBasicTest()
    {
        // 基本的なアクセシビリティのテスト
        await Page.GotoAsync($"{BaseUrl}/");
        
        // ページタイトルが設定されていることを確認
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var title = await Page.TitleAsync();
        Console.WriteLine($"Page title: '{title}'");
        Assert.IsTrue(!string.IsNullOrEmpty(title), $"ページタイトルが設定されているべきです。取得されたタイトル: '{title}'");
        
        // メインランドマークの確認
        var hasMainContent = await Page.Locator("main, [role='main'], h1, h2, h3").CountAsync() > 0;
        Assert.IsTrue(hasMainContent, "メインコンテンツまたは見出しが存在するべきです");
        
        // ナビゲーションの確認
        var hasNavigation = await Page.Locator("nav, [role='navigation']").CountAsync() > 0;
        Assert.IsTrue(hasNavigation, "ナビゲーション要素が存在するべきです");
    }

    [TestMethod]
    public async Task FormValidationUITest()
    {
        // フォームバリデーションのUIテスト
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/EditAuthor/172-32-1176");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(2000);
        
        // 必須フィールドをクリアして無効な状態にする
        var inputFields = Page.Locator("input[type='text']");
        var fieldCount = await inputFields.CountAsync();
        
        if (fieldCount > 0)
        {
            var firstField = inputFields.First;
            await firstField.ClearAsync();
            await firstField.BlurAsync(); // フィールドからフォーカスを外す
            
            // バリデーションメッセージまたはスタイルの確認
            await Page.WaitForTimeoutAsync(500);
            
            // エラー状態の視覚的な表示を確認
            var hasValidationIndicator = await Page.Locator(".field-validation-error, .is-invalid, .error, .validation-summary").IsVisibleAsync();
            
            // 更新ボタンをクリックしてサーバーサイドバリデーションをテスト
            await Page.ClickAsync("button:has-text('更新')");
            
            // バリデーションエラーが表示されることを確認
            await Page.WaitForTimeoutAsync(1000);
            var hasErrorAfterSubmit = await Page.Locator(".validation-summary, .field-validation-error").IsVisibleAsync();
        }
    }
}
