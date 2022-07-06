using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bnz.UI.Tests
{
    [TestFixture]
    public abstract class PlaywrightBase
    {
        protected static IPlaywright playwright;
        protected static IBrowser browser;
        protected static TargetedBrowser _targetedBrowser;

        protected IPage page;
        protected IBrowserContext context;
        private static string CurrentTestFolder = TestContext.CurrentContext.TestDirectory;
        private static DirectoryInfo LogsFolder;
        private static DirectoryInfo ScreenshotsFolder;

        public PlaywrightBase(TargetedBrowser targetedBrowser)
        {
            _targetedBrowser = targetedBrowser;
        }

        [OneTimeSetUp]
        public static void CreateiPlaywrightAndiBrowserContextInstances()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(BrowserSettings));
            var playwrightConfig = section.Get<BrowserSettings>();
            CurrentTestFolder = string.IsNullOrEmpty(playwrightConfig.LogFolderPath) ? CurrentTestFolder : playwrightConfig.LogFolderPath;
            LogsFolder = Directory.CreateDirectory(Path.Combine(CurrentTestFolder, "Logs"));
            ScreenshotsFolder = Directory.CreateDirectory(Path.Combine(CurrentTestFolder, "Screenshots"));
            browser = Task.Run(() => GetBrowserAsync(_targetedBrowser)).Result;
        }

        [SetUp]
        public async Task CreateBrowserContextAndPageContextInstances()
        {
            context = await browser.NewContextAsync();
            page = await context.NewPageAsync();
            await page.SetViewportSizeAsync(1920, 1080);
        }

        [TearDown]
        public async Task DisposeiPageContextAndiBrowserContextInstances()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (testResult.Status.Equals(TestStatus.Failed) || testResult.Status.Equals(ResultState.Error))
            {
                var testSpecificScreenshotFolder = Directory.CreateDirectory(Path.Combine(ScreenshotsFolder.FullName, TestContext.CurrentContext.Test.Name));
                var screenshotPath = Path.Combine(testSpecificScreenshotFolder.FullName, $"TestFailure_{DateTime.Now:yyyyMMddHHmmss}.png");
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
            }

            await context.CloseAsync();
        }

        [OneTimeTearDown]
        public void DisposeiBrowserContextAndiPlaywrightContextInstances()
        {
            playwright?.Dispose();
        }

        private static async Task<IBrowser> GetBrowserAsync(TargetedBrowser targetedBrowser)
        {
            playwright = await Playwright.CreateAsync();
            IBrowser browser = targetedBrowser switch
            {
                TargetedBrowser.Chrome => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "chrome",
                    Headless = false,
                    SlowMo = 2000
                }),
                TargetedBrowser.Firefox => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "firefox",
                    Headless = false,
                    SlowMo = 2000
                }),
                TargetedBrowser.Safari => await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "safari",
                    Headless = false,
                    SlowMo = 2000
                }),
                _ => throw new Exception("Invalid value for parameter named browser in the config file"),
            };
            return browser;
        }
    }
}