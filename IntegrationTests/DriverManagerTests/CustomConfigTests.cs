using System;
using IntegrationTests.BrowserTests;
using OpenQA.Selenium;
using WebDriverManager;
using Xunit;

namespace IntegrationTests.DriverManagerTests
{
    public class CustomConfigTests: IDisposable
    {
        private IWebDriver _webDriver;
        private string _driverExe;

        [Fact, Trait("Category", "Browser")]
        protected void CustomConfigTest()
        {
            _driverExe = "chromedriver";
            new DriverManager().SetUpDriver(new TaobaoChromeConfig());
            _webDriver = new DriverCreator().Create(DriverType.Chrome);
            _webDriver.Navigate().GoToUrl("https://www.wikipedia.org");
            Assert.Equal("Wikipedia", _webDriver.Title);
        }

        public void Dispose()
        {
            try
            {
                _webDriver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            finally
            {
                Helper.KillProcesses(_driverExe);
            }
        }
    }
}