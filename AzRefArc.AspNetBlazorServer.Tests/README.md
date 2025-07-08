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

## 使用テストフレームワーク

- **xUnit**: メインテストフレームワーク
- **bUnit**: Blazor コンポーネントテスト用
- **Microsoft.AspNetCore.Mvc.Testing**: 統合テスト用
- **Moq**: モッキングフレームワーク
- **Microsoft.EntityFrameworkCore.InMemory**: インメモリデータベーステスト用

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
```

## テスト作成時の注意点

1. **テストの命名規則**: メソッド名_状況_期待結果 の形式を使用
2. **テストの独立性**: 各テストは他のテストに依存しないよう作成
3. **AAA パターン**: Arrange-Act-Assert の順序でテストを構成
4. **テストデータの分離**: インメモリデータベースを使用してテストを分離

## 今後の拡張

- パフォーマンステストの追加
- セキュリティテストの追加
- ロードテストの追加
