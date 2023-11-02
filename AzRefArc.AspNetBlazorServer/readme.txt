■ 設計・実装のポイント

・プロジェクトテンプレートは InteractiveServer / global を選択
　－InteractiveServer を選択する理由
　　多くのページで DB が絡んだ対話型処理が必要になる → WASM + WebAPI では実装が大変
　－global を選択する理由
　　① 業務アプリの場合、ほぼすべてのページに会話型処理がある
　　　→ ページ単位でレンダリングモードを個々の開発者に選択させるメリットがあまりない
　　　→ 多少の性能損失があってもアーキテクチャ的に統一されていたほうが保守がしやすい
　　② perPage の場合、SignalR 回線がページ単位となる
　　　→ MainLayout 上で <ErrorBoundary> を使った集約例外ハンドラを仕掛けることができない

・プリレンダリングは無効化
　SEO 最適化が不要なため。
　CustomRenderingMode にて無効モードを用意し、_Imports.razor と App.razor にて指定。

・例外処理
　① 開発・運用共用
　　－<div id="blazor-error-ui"> を利用してエラーを通知
　　　→ ユーザが業務を続けられないように操作を抑止、リロードしかできないようにする
　　－例外ファイルログ出力を追加
　　　→ FileLogger から詳細な例外ログを出力
　　　→ ローカルのアプリ用フォルダに出力
　　　　（ローカル実行なら C:\Users\nakama\AppData\Local\AzRefArc.AspNetBlazorServer）
　② 開発中のみ
　　－appsettings.Development.json に "DetailedErrors": true を設定
　　　→ ブラウザコンソールでサーバ側で発生した例外のスタックトレースを確認できる

・NuGet パッケージ
　以下 2 つのみ追加
    <PackageReference Include="Microsoft.AspNetCore.Components.QuickGrid" Version="8.0.0-rc.2.23480.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0-rc.2.23480.1" />
　※ 推移パッケージである Azure.Identity に脆弱性があるため、このパッケージのみ暫定的にパッチ適用している

・Program.cs
　DI コンテナにはサービスとして PubsDbContext と FileLogger を追加。
　他はデフォルト設定のまま。

・DB アクセスカルチャ
　インバリアントカルチャを利用したいが、DB の仕様が古く、en-us カルチャが必要。
　このため 	<InvariantGlobalization>false</InvariantGlobalization> の設定が必要。
　この場合、軽量 Docker イメージの利用に制限が生じる
　alpine イメージだとカルチャデータが含まれていないため、ICU ライブラリのインストールが必要


