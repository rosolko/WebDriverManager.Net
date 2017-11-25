using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
            new object[] {new ChromeConfig(), DriverType.Chrome, "chromedriver", "chrome"},
//            new object[] {new EdgeConfig(), DriverType.Edge, "MicrosoftWebDriver", "MicrosoftEdge"},
            new object[] {new FirefoxConfig(), DriverType.Firefox, "geckodriver", "firefox"},
            new object[] {new InternetExplorerConfig(), DriverType.InternetExplorer, "IEDriverServer", "iexplore"},
            new object[] {new OperaConfig(), DriverType.Opera, "operadriver", "opera"},
            new object[] {new PhantomConfig(), DriverType.Phantom, "phantomjs", "phantomjs"}
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
        private string _driverExe;
        private string _browserExe;

        [Theory, ClassData(typeof(BrowserData)), Trait("Category", "Browser")]
        protected void BrowserTest(IDriverConfig driverConfig, DriverType driverType, string driverExe, string browserExe)
        {
            _driverExe = driverExe;
            _browserExe = browserExe;
            new DriverManager().SetUpDriver(driverConfig);
            _webDriver = new DriverCreator().Create(driverType);
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            Assert.Equal("Wikipedia", _webDriver.Title);
        }

        public void Dispose()
        {
            try
            {
                _webDriver.Close();
                _webDriver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            finally
            {
                Helper.KillProcesses(_driverExe);
                Helper.KillProcesses(_browserExe);
            }
        }
    }
}