# Playwright UI テスト実行スクリプト

# パッケージの復元
Write-Host "パッケージを復元しています..." -ForegroundColor Green
dotnet restore

# Playwrightブラウザのインストール
Write-Host "Playwrightブラウザをインストールしています..." -ForegroundColor Green
pwsh -c "dotnet tool install --global Microsoft.Playwright.CLI"
playwright install

# アプリケーションをバックグラウンドで起動
Write-Host "アプリケーションを起動しています..." -ForegroundColor Green
$app = Start-Process -FilePath "dotnet" -ArgumentList "run --project ../AzRefArc.AspNetBlazorServer/AzRefArc.AspNetBlazorServer.csproj" -PassThru

# アプリケーションの起動を待機
Write-Host "アプリケーションの起動を待機しています..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

try {
    # HTTPSエンドポイントをテスト
    $response = Invoke-WebRequest -Uri "https://localhost:7268" -SkipCertificateCheck -ErrorAction SilentlyContinue
    if ($response.StatusCode -eq 200) {
        Write-Host "アプリケーションが正常に起動しました" -ForegroundColor Green
    } else {
        Write-Host "アプリケーションの起動確認に失敗しました" -ForegroundColor Red
    }
} catch {
    Write-Host "アプリケーションへの接続テストでエラーが発生しました: $_" -ForegroundColor Red
}

try {
    # Playwrightテストを実行
    Write-Host "Playwrightテストを実行しています..." -ForegroundColor Green
    dotnet test --filter "Category=Playwright" --logger "console;verbosity=detailed" --settings test.runsettings
} finally {
    # アプリケーションプロセスを終了
    Write-Host "アプリケーションを停止しています..." -ForegroundColor Yellow
    Stop-Process -Id $app.Id -Force -ErrorAction SilentlyContinue
}

Write-Host "テスト実行が完了しました" -ForegroundColor Green
