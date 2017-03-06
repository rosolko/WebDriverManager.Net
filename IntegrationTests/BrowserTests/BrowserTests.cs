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
            new object[] {new EdgeConfig(), DriverType.Edge},
            new object[] {new FirefoxConfig(), DriverType.Firefox},
            new object[] {new IEConfig(), DriverType.IE},
            new object[] {new OperaConfig(), DriverType.Opera},
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
            if (driverType == DriverType.Phantom)
            {
                new DriverManager().SetUpDriver(driverConfig, "2.1.1");
            }
            else
            {
                new DriverManager().SetUpDriver(driverConfig);
            }
            _webDriver = new DriverCreator().Create(driverType);
            _webDriver.Navigate().GoToUrl("https://www.google.com/ncr");
            Assert.True(_webDriver.Title.Contains("Google"));
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }
    }
}