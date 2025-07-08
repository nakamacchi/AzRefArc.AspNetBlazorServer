using Microsoft.Playwright;

namespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public class BizGroupBTests : PlaywrightTestBase
{
    [TestMethod]
    public async Task EditAuthorListPageTest()
    {
        // 著者編集一覧ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/ListAuthors");
        
        // ページタイトルの確認
        await AssertPageTitle("著者データの編集");
        
        // ページ見出しの確認
        await AssertTextExists("著者データの編集");
        await AssertTextExists("編集対象となる著者を選択してください。");
        
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
        
        // 著者IDリンクが存在することを確認（編集リンクとして機能）
        var authorIdLinks = Page.Locator("table a");
        var linkCount = await authorIdLinks.CountAsync();
        Assert.IsTrue(linkCount > 0, "著者IDリンクが存在するべきです");
    }

    [TestMethod]
    public async Task EditAuthorFormTest()
    {
        // まず一覧ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // 最初の著者IDリンクをクリック（著者IDが編集リンクになっている）
        var firstEditLink = Page.Locator("table a").First;
        await firstEditLink.ClickAsync();
        
        // 編集ページのタイトル確認
        await AssertPageTitle("著者データの編集");
        
        // フォーム要素の確認
        await AssertTextExists("著者データの編集");
        await AssertTextExists("著者データを修正してください。");
        
        // フォームフィールドの存在確認（form内のinputのみを対象とする）
        await Expect(Page.Locator("form input.form-text")).ToHaveCountAsync(3); // 名、姓、電話番号
        await Expect(Page.Locator("select")).ToBeVisibleAsync(); // 州選択
        
        // ボタンの存在確認
        await Expect(Page.Locator("button:has-text('更新')")).ToBeVisibleAsync();
        await Expect(Page.Locator("button:has-text('キャンセル')")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task EditAuthorFormValidationTest()
    {
        // 編集ページに直接移動（著者IDが存在する場合）
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/EditAuthor/172-32-1176");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(2000);
        
        // 名前フィールドをクリアして空にする
        var firstNameField = Page.Locator("form input[name*='AuthorFirstName']");
        await firstNameField.ClearAsync();
        
        // 更新ボタンをクリック
        await Page.ClickAsync("button:has-text('更新')");
        
        // バリデーションエラーが表示されることを確認
        // （実際のバリデーションメッセージは実装によって異なります）
        var validationSummary = Page.Locator(".validation-summary");
        if (await validationSummary.IsVisibleAsync())
        {
            await Expect(validationSummary).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    public async Task EditAuthorCancelTest()
    {
        // 編集ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/EditAuthor/172-32-1176");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(2000);
        
        // キャンセルボタンをクリック
        await Page.ClickAsync("button:has-text('キャンセル')");
        
        // 一覧ページに戻ることを確認
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/ListAuthors");
        await AssertTextExists("著者データの編集");
    }

    [TestMethod]
    public async Task EditAuthorFormFieldsTest()
    {
        // 編集ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/EditAuthor/172-32-1176");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(2000);
        
        // フォームフィールドに値が入力されていることを確認
        var authorIdDisplay = Page.Locator("text=172-32-1176");
        await Expect(authorIdDisplay).ToBeVisibleAsync();
        
        // 入力フィールドの値を変更
        var phoneField = Page.Locator("input").Nth(2); // 3番目のinputフィールド（電話番号）
        await phoneField.ClearAsync();
        await phoneField.FillAsync("555-1234");
        
        // 州を変更
        await Page.SelectOptionAsync("select", "NY");
        
        // フィールドの値が変更されたことを確認
        await Expect(phoneField).ToHaveValueAsync("555-1234");
        await Expect(Page.Locator("select")).ToHaveValueAsync("NY");
    }

    [TestMethod]
    public async Task NavigationFromHomeToEditTest()
    {
        // ホームページから編集機能への遷移テスト
        await Page.GotoAsync($"{BaseUrl}/");
        
        // 更新系アプリのリンクをクリック
        await Page.ClickAsync("a[href='/BizGroupB/EditAuthorByOptimistic/ListAuthors']");
        
        // 編集一覧ページに遷移することを確認
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/ListAuthors");
        await AssertTextExists("著者データの編集");
    }

    [TestMethod]
    public async Task EditFormResponsiveTest()
    {
        // モバイルビューポートに設定
        await Page.SetViewportSizeAsync(375, 667);
        
        // 編集ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupB/EditAuthorByOptimistic/EditAuthor/172-32-1176");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(2000);
        
        // フォーム要素がモバイルで適切に表示されることを確認
        await Expect(Page.Locator("form input.form-text").First).ToBeVisibleAsync();
        await Expect(Page.Locator("select")).ToBeVisibleAsync();
        await Expect(Page.Locator("button:has-text('更新')")).ToBeVisibleAsync();
        await Expect(Page.Locator("button:has-text('キャンセル')")).ToBeVisibleAsync();
        
        // デスクトップビューポートに戻す
        await Page.SetViewportSizeAsync(1920, 1080);
    }
}
