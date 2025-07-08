# Playwright UI テスト実行スクリプト

# パッケージの復元
Write-Host "パッケージを復元しています..." -ForegroundColor Green
dotnet restore

# Playwrightブラウザのインストール
Write-Host "Playwrightブラウザをインストールしています..." -ForegroundColor Green
dotnet tool install --global Microsoft.Playwright.CLI --ignore-failed-sources
dotnet build
pwsh -c "playwright install" -ErrorAction SilentlyContinue

# アプリケーションをバックグラウンドで起動
Write-Host "アプリケーションを起動しています..." -ForegroundColor Green
$app = Start-Process -FilePath "dotnet" -ArgumentList "run --project ../AzRefArc.AspNetBlazorServer/AzRefArc.AspNetBlazorServer.csproj" -PassThru

# アプリケーションの起動を待機
Write-Host "アプリケーションの起動を待機しています..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

try {
    # HTTPSエンドポイントをテスト
    $response = Invoke-WebRequest -Uri "https://localhost:7268" -SkipCertificateCheck:$true -ErrorAction SilentlyContinue
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
    dotnet test --filter "FullyQualifiedName~PlaywrightTests" --logger "console;verbosity=detailed" --settings test.runsettings
    
    # テスト結果の保存
    Write-Host "テスト結果とスクリーンショットを保存しています..." -ForegroundColor Green
    
    # TestResultsフォルダ下にタイムスタンプ付きのディレクトリ作成
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $testResultsBase = "TestResults"
    $resultsDir = "$testResultsBase\playwright-results-$timestamp"
    New-Item -ItemType Directory -Path $resultsDir -Force | Out-Null
    
    # TestResults/playwright-reportディレクトリが存在する場合はコピー
    if (Test-Path "TestResults\playwright-report") {
        Copy-Item -Path "TestResults\playwright-report" -Destination "$resultsDir\playwright-report" -Recurse -Force
        Write-Host "HTMLレポートを保存しました: $resultsDir\playwright-report" -ForegroundColor Green
    }
    
    # TestResults/test-resultsディレクトリが存在する場合はコピー（従来のtest-resultsも確認）
    if (Test-Path "TestResults") {
        $testResultsItems = Get-ChildItem "TestResults" -Exclude "playwright-results-*"
        foreach ($item in $testResultsItems) {
            if ($item.Name -ne "playwright-report") {  # playwright-reportは既にコピー済み
                Copy-Item -Path $item.FullName -Destination "$resultsDir\$($item.Name)" -Recurse -Force
            }
        }
        Write-Host "Playwrightテスト結果を保存しました: $resultsDir" -ForegroundColor Green
    }
    
    # 従来のtest-resultsディレクトリも確認（後方互換性のため）
    if (Test-Path "test-results") {
        Copy-Item -Path "test-results" -Destination "$resultsDir\legacy-test-results" -Recurse -Force
        Write-Host "従来のテスト結果を保存しました: $resultsDir\legacy-test-results" -ForegroundColor Green
    }
    
    # C#テストの実行時に生成されたスクリーンショットとビデオを保存
    $binTestResultsPath = "bin\Debug\net8.0\TestResults"
    if (Test-Path $binTestResultsPath) {
        Copy-Item -Path $binTestResultsPath -Destination "$resultsDir\csharp-test-results" -Recurse -Force
        Write-Host "C#テストのスクリーンショットとビデオを保存しました: $resultsDir\csharp-test-results" -ForegroundColor Green
    }
    
    Write-Host "テスト結果は以下に保存されました: $resultsDir" -ForegroundColor Cyan
    
} finally {
    # アプリケーションプロセスを終了
    Write-Host "アプリケーションを停止しています..." -ForegroundColor Yellow
    Stop-Process -Id $app.Id -Force -ErrorAction SilentlyContinue
}

Write-Host "テスト実行が完了しました" -ForegroundColor Green
