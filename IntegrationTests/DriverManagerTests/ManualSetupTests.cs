using System;
using System.IO;
using IntegrationTests.BrowserTests;
using OpenQA.Selenium;
using WebDriverManager;
using Xunit;

namespace IntegrationTests.DriverManagerTests
{
    public class ManualSetupTests : IDisposable
    {
        private IWebDriver _webDriver;
        private readonly string _url;
        private readonly string _binaryOutput;
        private readonly string _driverName;

        public ManualSetupTests()
        {
            _url = "https://chromedriver.storage.googleapis.com/2.25/chromedriver_win32.zip";
            _binaryOutput = Path.Combine(Directory.GetCurrentDirectory(), "Chrome", "2.25", "X32", "chromedriver.exe");
            _driverName = "chromedriver.exe";
        }

        [Fact, Trait("Category", "Browser")]
        protected void ManualSetupTest()
        {
            new DriverManager().SetUpDriver(_url, _binaryOutput, _driverName);
            _webDriver = new DriverCreator().Create(DriverType.Chrome);
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            Assert.Equal("Wikipedia", _webDriver.Title);
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }
    }
}