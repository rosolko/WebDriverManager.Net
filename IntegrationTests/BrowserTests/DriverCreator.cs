using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.PhantomJS;

namespace IntegrationTests.BrowserTests
{
    public class DriverCreator
    {
        public IWebDriver Create(DriverType driverType)
        {
            switch (driverType)
            {
                case DriverType.Chrome:
                {
                    return new ChromeDriver();
                }
                case DriverType.Edge:
                {
                    return new EdgeDriver();
                }
                case DriverType.Firefox:
                {
                    return new FirefoxDriver();
                }
                case DriverType.IE:
                {
                    return new InternetExplorerDriver();
                }
                case DriverType.Opera:
                {
                    return new OperaDriver();
                }
                case DriverType.Phantom:
                {
                    return new PhantomJSDriver();
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(driverType), driverType, null);
                }
            }
        }
    }
}