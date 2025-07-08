using Microsoft.Playwright;

namespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public class BizGroupATests : PlaywrightTestBase
{
    [TestMethod]
    public async Task HomePageNavigationTest()
    {
        // ホームページに移動
        await Page.GotoAsync($"{BaseUrl}/");
        
        // ページタイトルの確認
        await AssertPageTitle("ASP.NET Core Blazor Server 業務サンプルアプリケーション");
        
        // 各業務グループへのリンクが存在することを確認
        await AssertTextExists("Server 参照系アプリ");
        await AssertTextExists("Server 更新系アプリ");
        await AssertTextExists("Server QuickGrid による実装アプリ");
        await Expect(Page.Locator("a[href='/#Utilities']")).ToBeVisibleAsync();
        
        // テスト成功時のスクリーンショットを撮影
        await TakeScreenshotAsync("HomePageNavigationTest");
    }

    [TestMethod]
    public async Task ShowAllAuthorsPageTest()
    {
        // 全著者データ表示ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAllAuthors/ListAuthors");
        
        // ページタイトルの確認
        await AssertPageTitle("著者データ一覧");
        
        // ページ見出しの確認
        await AssertTextExists("著者データ一覧");
        
        // データロード中の表示を確認（最初の2秒間）
        await AssertTextExists("現在データを取得中です...");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // テーブルが表示されることを確認
        await Expect(Page.Locator("table.table")).ToBeVisibleAsync();
        
        // テーブルヘッダーの確認
        await AssertTextExists("著者ID");
        await AssertTextExists("著者名");
        await AssertTextExists("電話番号");
        await AssertTextExists("州");
        await AssertTextExists("契約有無");
        
        // データ行が存在することを確認（テストデータが存在する場合）
        var dataRows = Page.Locator("table tbody tr");
        var rowCount = await dataRows.CountAsync();
        Assert.IsTrue(rowCount > 0, "テーブルにデータ行が存在するべきです");
    }

    [TestMethod]
    public async Task FilterByStatePageTest()
    {
        // 州による著者データ検索ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAuthorsByState/FilterByState");
        
        // ページタイトルの確認
        await AssertPageTitle("州による著者一覧表示");
        
        // ページ見出しの確認
        await AssertTextExists("州による著者一覧表示");
        await AssertTextExists("州を指定してください。");
        
        // UI要素の存在確認
        await Expect(Page.Locator("select.form-select-sm")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[value='データ取得']")).ToBeVisibleAsync();
        
        // 州を選択
        await Page.SelectOptionAsync("select.form-select-sm", "CA");
        
        // データ取得ボタンをクリック
        await Page.ClickAsync("input[value='データ取得']");
        
        // データロード完了まで待機
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // 結果テーブルまたはメッセージの表示を確認
        var hasTable = await Page.Locator("table.table").IsVisibleAsync();
        var hasNoDataMessage = await Page.Locator("text=データがありません。").IsVisibleAsync();
        
        Assert.IsTrue(hasTable || hasNoDataMessage, "テーブルまたは'データがありません'メッセージが表示されるべきです");
    }

    [TestMethod]
    public async Task NavigationMenuTest()
    {
        // ホームページに移動
        await Page.GotoAsync($"{BaseUrl}/");
        
        // ナビゲーションメニューの確認
        await Expect(Page.Locator("text=Home")).ToBeVisibleAsync();
        await Expect(Page.Locator(".nav-link:has-text('参照系アプリ')")).ToBeVisibleAsync();
        await Expect(Page.Locator(".nav-link:has-text('更新系アプリ')")).ToBeVisibleAsync();
        await Expect(Page.Locator(".nav-link:has-text('QuickGrid 実装')")).ToBeVisibleAsync();
        await Expect(Page.Locator(".nav-link:has-text('ユーティリティ')")).ToBeVisibleAsync();
        
        // 参照系アプリのリンクをクリック
        await Page.ClickAsync("text=参照系アプリ");
        
        // URLが変更されることを確認
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/#BizGroupA");
    }

    [TestMethod]
    public async Task FilterByStateWithSortPageTest()
    {
        // ソート機能付きの州による著者データ検索ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAuthorsByState/FilterByStateWithSort");
        
        // ページタイトルの確認
        await AssertPageTitle("州による著者一覧表示");
        
        // ページ見出しの確認
        await AssertTextExists("州による著者一覧表示");
        
        // UI要素の存在確認
        await Expect(Page.Locator("select")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[value='データ取得']")).ToBeVisibleAsync();
        
        // 州を選択してデータ取得
        await Page.SelectOptionAsync("select", "CA");
        await Page.ClickAsync("input[value='データ取得']");
        
        // データロード完了まで待機
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // ソート機能がある場合のテーブルヘッダーリンクの確認
        var tableHeaders = Page.Locator("table thead th");
        var headerCount = await tableHeaders.CountAsync();
        Assert.IsTrue(headerCount > 0, "テーブルヘッダーが存在するべきです");
    }

    [TestMethod]
    public async Task ResponsiveDesignTest()
    {
        // モバイルビューポートに設定
        await Page.SetViewportSizeAsync(375, 667);
        
        // 全著者データ表示ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupA/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // レスポンシブテーブルクラスの確認
        await Expect(Page.Locator(".table-responsive")).ToBeVisibleAsync();
        
        // デスクトップビューポートに戻す
        await Page.SetViewportSizeAsync(1920, 1080);
        
        // テーブルが正常に表示されることを確認
        await Expect(Page.Locator("table.table")).ToBeVisibleAsync();
    }
}
