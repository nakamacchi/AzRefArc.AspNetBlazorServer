# AzRefArc.AspNetBlazorServer.Tests

このプロジェクトは、AzRefArc.AspNetBlazorServer アプリケーションの単体テスト、統合テスト、およびコンポーネントテストを含んでいます。

## テストの種類

### 1. 単体テスト (UnitTests)
- データアクセス層のテスト
- ユーティリティクラスのテスト
- ビジネスロジックのテスト

### 2. 統合テスト (IntegrationTests)
- Web APIエンドポイントのテスト
- データベース統合テスト
- 全体的なアプリケーションフローテスト

### 3. コンポーネントテスト (ComponentTests)
- Blazor コンポーネントのレンダリングテスト
- ユーザーインタラクションテスト
- UI コンポーネントの動作テスト

### 4. Playwright UIテスト (PlaywrightTests)
- エンドツーエンドの UI テスト
- ブラウザでの実際のユーザー操作をシミュレート
- 業務ページの機能テスト
- レスポンシブデザインテスト
- フォームバリデーションテスト

## 使用テストフレームワーク

- **MSTest**: Microsoftの標準テストフレームワーク
- **bUnit**: Blazor コンポーネントテスト用
- **Microsoft.AspNetCore.Mvc.Testing**: 統合テスト用
- **Moq**: モッキングフレームワーク
- **Microsoft.EntityFrameworkCore.InMemory**: インメモリデータベーステスト用
- **Microsoft.Playwright**: ブラウザ自動化とUIテスト用

## テストの実行方法

### Visual Studio から実行
1. テストエクスプローラーを開く
2. テストを選択して実行

### コマンドラインから実行
```bash
# すべてのテストを実行
dotnet test

# 特定のテストプロジェクトを実行
dotnet test AzRefArc.AspNetBlazorServer.Tests

# カバレッジレポート付きで実行
dotnet test --collect:"XPlat Code Coverage"

# Playwrightテストのみを実行
dotnet test --filter "Category=Playwright"

# PlaywrightテストをPowerShellスクリプトで実行
.\run-playwright-tests.ps1
```

### Playwrightテストの初期設定

```bash
# Playwright CLIをインストール
dotnet tool install --global Microsoft.Playwright.CLI

# ブラウザをインストール
playwright install
```

## テスト作成時の注意点

1. **テストの命名規則**: メソッド名_状況_期待結果 の形式を使用
2. **テストの独立性**: 各テストは他のテストに依存しないよう作成
3. **AAA パターン**: Arrange-Act-Assert の順序でテストを構成
4. **テストデータの分離**: インメモリデータベースを使用してテストを分離
5. **Playwrightテスト**: 実際のアプリケーションが起動している状態で実行

## Playwrightテストの特徴

- **実ブラウザでのテスト**: Chrome、Firefox、Safari での実際の動作をテスト
- **視覚的テスト**: スクリーンショットや動画記録による検証
- **レスポンシブテスト**: 異なる画面サイズでの動作確認
- **パフォーマンステスト**: ページロード時間の測定
- **アクセシビリティテスト**: 基本的なアクセシビリティの確認

## テストカバレッジ

### BizGroupA (参照系アプリ)
- 全著者データ表示
- 州による著者データ検索
- ソート機能付き検索
- ナビゲーション機能

### BizGroupB (更新系アプリ)
- 著者データ編集フォーム
- フォームバリデーション
- 楽観的同時実行制御

### BizGroupC (QuickGrid実装)
- QuickGridコンポーネント
- ソート・ページネーション
- 複雑な検索条件
- 詳細情報表示

### エラーハンドリング・ユーティリティ
- エラーページ表示
- 診断情報表示
- 例外処理テスト
- セキュリティヘッダー確認

## 今後の拡張

- より詳細なパフォーマンステストの追加
- セキュリティテストの強化
- ロードテストの追加
- 視覚的回帰テストの実装
- API テストの追加
