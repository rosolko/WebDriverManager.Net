using System;
using IntegrationTests.BrowserTests;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Services.Impl;
using Xunit;

namespace IntegrationTests.DriverManagerTests
{
    public class CustomServiceTests: IDisposable
    {
        private IWebDriver _webDriver;
        private readonly BinaryService _customBinaryService;
        private readonly VariableService _customVariableService;

        public CustomServiceTests()
        {
            _customBinaryService = new BinaryService();
            _customVariableService = new VariableService();
        }

        [Fact, Trait("Category", "Browser")]
        protected void CustomServiceTest()
        {
            new DriverManager(_customBinaryService, _customVariableService).SetUpDriver(new FirefoxConfig());
            _webDriver = new DriverCreator().Create(DriverType.Firefox);
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            Assert.Equal("Wikipedia", _webDriver.Title);
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }
    }
}