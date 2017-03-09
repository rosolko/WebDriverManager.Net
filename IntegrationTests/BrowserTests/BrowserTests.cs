using System;
using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace IntegrationTests.BrowserTests
{
    public class BrowserData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {new ChromeConfig(), DriverType.Chrome},
//            new object[] {new EdgeConfig(), DriverType.Edge},
            new object[] {new FirefoxConfig(), DriverType.Firefox},
            new object[] {new InternetExplorerConfig(), DriverType.InternetExplorer},
//            new object[] {new OperaConfig(), DriverType.Opera},
            new object[] {new PhantomConfig(), DriverType.Phantom}
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class BrowserTests : IDisposable
    {
        private IWebDriver _webDriver;

        [Theory, ClassData(typeof(BrowserData)), Trait("Category", "Browser")]
        protected void BrowserTest(IDriverConfig driverConfig, DriverType driverType)
        {
            new DriverManager().SetUpDriver(driverConfig);
            _webDriver = new DriverCreator().Create(driverType);
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            Assert.Equal("Wikipedia", _webDriver.Title);
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }
    }
}