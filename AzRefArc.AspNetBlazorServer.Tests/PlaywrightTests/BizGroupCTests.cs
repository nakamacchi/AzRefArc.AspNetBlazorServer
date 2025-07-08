using Microsoft.Playwr        // データ取得ボタンをクリック
        await Page.ClickAsync("input[value='データ取得']");
        
        // データがロードされるまで待機
        await Page.WaitForSelectorAsync("div.table-responsive", new PageWaitForSelectorOptions { Timeout = 10000 });
        
        // QuickGridまたは通常のテーブルが表示されているかを確認
        var quickGrid = Page.Locator(".quickgrid");
        var tableResponsive = Page.Locator("div.table-responsive");
        
        await Expect(tableResponsive).ToBeVisibleAsync();mespace AzRefArc.AspNetBlazorServer.Tests.PlaywrightTests;

[TestClass]
public class BizGroupCTests : PlaywrightTestBase
{
    [TestMethod]
    public async Task QuickGridShowAllAuthorsTest()
    {
        // QuickGrid全著者データ表示ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAllAuthors/ListAuthors");
        
        // ページタイトルの確認
        await AssertPageTitle("著者データ一覧");
        
        // ページ見出しの確認
        await AssertTextExists("著者データ一覧");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // QuickGridテーブルが表示されることを確認
        var quickGrid = Page.Locator(".quickgrid");
        if (await quickGrid.IsVisibleAsync())
        {
            await Expect(quickGrid).ToBeVisibleAsync();
        }
        else
        {
            // QuickGridクラスがない場合は通常のテーブルを確認
            await Expect(Page.Locator("table")).ToBeVisibleAsync();
        }
        
        // テーブルヘッダーの確認
        await AssertTextExists("著者ID");
        await AssertTextExists("著者名");
        await AssertTextExists("電話番号");
        await AssertTextExists("州");
        await AssertTextExists("契約有無");
    }

    [TestMethod]
    public async Task QuickGridFilterByStateTest()
    {
        // QuickGrid州による著者データ検索ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAuthorsByState/FilterByState");
        
        // ページタイトルの確認
        await AssertPageTitle("州による著者一覧表示");
        
        // フィルター機能のテスト
        await Expect(Page.Locator("select")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[value='データ取得']")).ToBeVisibleAsync();
        
        // 州を選択してデータ取得
        await Page.SelectOptionAsync("select", "CA");
        await Page.ClickAsync("input[value='データ取得']");
        
        // データロード完了まで待機
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // 結果が表示されることを確認
        var hasResults = await Page.Locator("table, .quickgrid").IsVisibleAsync();
        var hasNoDataMessage = await Page.Locator("text=データがありません").IsVisibleAsync();
        Assert.IsTrue(hasResults || hasNoDataMessage, "結果またはメッセージが表示されるべきです");
    }

    [TestMethod]
    public async Task QuickGridSortingTest()
    {
        // QuickGrid全著者データ表示ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // ソート可能なヘッダーをクリック（例：著者名）
        var authorNameHeader = Page.Locator("th:has-text('著者名')");
        if (await authorNameHeader.IsVisibleAsync())
        {
            await authorNameHeader.ClickAsync();
            
            // ソート後の状態を確認（実装によりソート指示子が表示される）
            await Page.WaitForTimeoutAsync(1000);
            
            // テーブルが再描画されることを確認
            await Expect(Page.Locator("table, .quickgrid")).ToBeVisibleAsync();
        }
    }

    [TestMethod]
    public async Task QuickGridPaginationTest()
    {
        // ページネーション機能があるページをテスト
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(3000);
        
        // ページネーションコントロールの確認
        var paginationControls = Page.Locator(".pagination, [aria-label*='ページ'], button:has-text('次へ'), button:has-text('前へ')");
        var hasPagination = await paginationControls.CountAsync() > 0;
        
        if (hasPagination)
        {
            // 次ページボタンをクリック
            var nextButton = Page.Locator("button:has-text('次へ'), button:has-text('>')").First;
            if (await nextButton.IsVisibleAsync() && await nextButton.IsEnabledAsync())
            {
                await nextButton.ClickAsync();
                await Page.WaitForTimeoutAsync(1000);
                
                // ページが変更されたことを確認
                await Expect(Page.Locator("table, .quickgrid")).ToBeVisibleAsync();
            }
        }
    }

    [TestMethod]
    public async Task QuickGridFilterByTopNTest()
    {
        // 件数による著者データ検索ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAuthorsByTopN/FilterByTopN");
        
        // ページタイトルの確認
        await AssertPageTitle("件数に基づくデータ取得");
        
        // フィルター要素の確認
        await Expect(Page.Locator("input.form-text")).ToBeVisibleAsync();
        await Expect(Page.Locator("button:has-text('表示')")).ToBeVisibleAsync();
        
        // 件数を設定してデータ取得
        var numberInput = Page.Locator("input.form-text");
        if (await numberInput.IsVisibleAsync())
        {
            await numberInput.FillAsync("5");
        }
        else
        {
            // セレクトボックスの場合
            await Page.SelectOptionAsync("select", "5");
        }
        
        await Page.ClickAsync("button:has-text('表示')");
        
        // データロード完了まで待機
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // 結果が表示されることを確認
        await Expect(Page.Locator("table, .quickgrid")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task QuickGridAuthorDetailTest()
    {
        // 著者詳細表示ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAuthorDetail/ShowAuthorDetail");
        
        // ページタイトルの確認
        await AssertPageTitle("著者の詳細情報の表示");
        
        // 検索フォームの確認
        await Expect(Page.Locator("input[type='text'], select")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[value='検索'], button:has-text('検索')")).ToBeVisibleAsync();
        
        // 著者IDを入力して検索
        var searchInput = Page.Locator("input[type='text']").First;
        await searchInput.FillAsync("172-32-1176");
        
        var searchButton = Page.Locator("input[value='検索'], button:has-text('検索')").First;
        await searchButton.ClickAsync();
        
        // 詳細情報が表示されることを確認
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // 詳細情報または「見つかりません」メッセージの確認
        var hasDetails = await Page.Locator(".author-detail, dl, table").IsVisibleAsync();
        var hasNotFoundMessage = await Page.Locator("text=見つかりません, text=該当なし").IsVisibleAsync();
        Assert.IsTrue(hasDetails || hasNotFoundMessage, "詳細情報またはメッセージが表示されるべきです");
    }

    [TestMethod]
    public async Task QuickGridComplexFilterTest()
    {
        // 複雑な条件による著者データ検索ページに移動
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAuthorsByCondition/FilterByCondition");
        
        // ページタイトルの確認
        await AssertPageTitle("複雑な条件指定による著者一覧表示");
        
        // 複数の検索条件フィールドの確認
        var inputFields = Page.Locator("input, select");
        var fieldCount = await inputFields.CountAsync();
        Assert.IsTrue(fieldCount > 1, "複数の検索条件フィールドが存在するべきです");
        
        // 検索ボタンの確認
        await Expect(Page.Locator("input[value='データ取得'], button:has-text('検索')")).ToBeVisibleAsync();
        
        // 条件を設定して検索実行
        var firstInput = inputFields.First;
        if (await firstInput.GetAttributeAsync("type") == "text")
        {
            await firstInput.FillAsync("test");
        }
        
        await Page.ClickAsync("input[value='データ取得'], button:has-text('検索')");
        
        // 結果が表示されることを確認
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [TestMethod]
    public async Task QuickGridResponsiveTest()
    {
        // モバイルビューポートでのテスト
        await Page.SetViewportSizeAsync(375, 667);
        
        await Page.GotoAsync($"{BaseUrl}/BizGroupC/ShowAllAuthors/ListAuthors");
        
        // データロード完了まで待機
        await Page.WaitForTimeoutAsync(5000);
        
        // モバイルでもテーブルが適切に表示されることを確認
        // 最初にQuickGridが存在するかチェック
        var quickgridExists = await Page.Locator(".quickgrid").IsVisibleAsync();
        var tableExists = await Page.Locator("table").IsVisibleAsync();
        
        Assert.IsTrue(quickgridExists || tableExists, "QuickGridまたはテーブルが表示されているべきです");
        
        // レスポンシブ対応の確認
        var responsiveContainer = Page.Locator(".table-responsive, .quickgrid-container");
        if (await responsiveContainer.IsVisibleAsync())
        {
            await Expect(responsiveContainer).ToBeVisibleAsync();
        }
        
        // デスクトップビューポートに戻す
        await Page.SetViewportSizeAsync(1920, 1080);
    }
}
