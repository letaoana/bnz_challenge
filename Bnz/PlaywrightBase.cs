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
        protected static IPlaywright Playwright;
        protected static IBrowser Browser;
        protected static TargetedBrowser TargetedBrowser;

        protected IPage Page;
        protected IBrowserContext Context;
        private static string _currentTestFolder = TestContext.CurrentContext.TestDirectory;
        private static DirectoryInfo _screenshotsFolder;

        protected PlaywrightBase(TargetedBrowser targetedBrowser)
        {
            TargetedBrowser = targetedBrowser;
        }

        [OneTimeSetUp]
        public static void CreateiPlaywrightAndiBrowserContextInstances()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(BrowserSettings));
            var playwrightConfig = section.Get<BrowserSettings>();
            _currentTestFolder = string.IsNullOrEmpty(playwrightConfig.LogFolderPath) ? _currentTestFolder : playwrightConfig.LogFolderPath;
            Directory.CreateDirectory(Path.Combine(_currentTestFolder, "Logs"));
            _screenshotsFolder = Directory.CreateDirectory(Path.Combine(_currentTestFolder, "Screenshots"));
            Browser = Task.Run(() => GetBrowserAsync(TargetedBrowser)).Result;
        }

        [SetUp]
        public async Task CreateBrowserContextAndPageContextInstances()
        {
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            await Page.SetViewportSizeAsync(1920, 1080);
        }

        [TearDown]
        public async Task DisposeiPageContextAndiBrowserContextInstances()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome;

            if (testResult.Status.Equals(TestStatus.Failed))
            {
                var testSpecificScreenshotFolder = Directory.CreateDirectory(Path.Combine(_screenshotsFolder.FullName, TestContext.CurrentContext.Test.Name));
                var screenshotPath = Path.Combine(testSpecificScreenshotFolder.FullName, $"TestFailure_{DateTime.Now:yyyyMMddHHmmss}.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
            }

            await Context.CloseAsync();
        }

        [OneTimeTearDown]
        public void DisposeiBrowserContextAndiPlaywrightContextInstances()
        {
            Playwright?.Dispose();
        }

        private static async Task<IBrowser> GetBrowserAsync(TargetedBrowser targetedBrowser)
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var browser = targetedBrowser switch
            {
                TargetedBrowser.Chrome => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "chrome",
                    Headless = false,
                    SlowMo = 2000
                }),
                TargetedBrowser.Firefox => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Channel = "firefox",
                    Headless = false,
                    SlowMo = 2000
                }),
                TargetedBrowser.Safari => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
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