import { PlaywrightTestConfig, devices } from '@playwright/test';

const config: PlaywrightTestConfig = {
  testDir: './PlaywrightTests',
  timeout: 30000,
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : 4,
  reporter: [
    ['html', { outputFolder: 'TestResults/playwright-report', open: 'never' }],
    ['junit', { outputFile: 'TestResults/junit.xml' }],
    ['json', { outputFile: 'TestResults/results.json' }]
  ],
  use: {
    baseURL: 'https://localhost:7268',
    ignoreHTTPSErrors: true,
    trace: 'retain-on-failure',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    headless: true, // ヘッドレスモードで実行
  },
  projects: [
    {
      name: 'msedge',
      use: {
        ...devices['Desktop Edge'],
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
