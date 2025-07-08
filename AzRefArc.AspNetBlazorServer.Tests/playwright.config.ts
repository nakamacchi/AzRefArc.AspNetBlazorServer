import { PlaywrightTestConfig } from '@playwright/test';

const config: PlaywrightTestConfig = {
  testDir: './PlaywrightTests',
  timeout: 30000,
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : 4,
  reporter: 'html',
  use: {
    baseURL: 'https://localhost:7268',
    ignoreHTTPSErrors: true,
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    headless: true, // ヘッドレスモードで実行
    slowMo: 1000, // 操作を1秒間隔で実行（見やすくするため）
  },
  projects: [
    {
      name: 'msedge',
      use: {
        ...require('@playwright/test').devices['Desktop Edge'],
        channel: 'msedge',
      },
    },
  ],
  webServer: {
    command: 'dotnet run --project ../AzRefArc.AspNetBlazorServer/AzRefArc.AspNetBlazorServer.csproj',
    port: 7268,
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
};

export default config;
