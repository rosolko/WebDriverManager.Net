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
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--no-sandbox");
                    return new ChromeDriver(chromeOptions);
                }
                case DriverType.Edge:
                {
                    var options = new EdgeOptions
                    {
                        PageLoadStrategy = EdgePageLoadStrategy.Eager
                    };
                    return new EdgeDriver(options);
                }
                case DriverType.Firefox:
                {
                    var firefoxProfile = new FirefoxProfile
                    {
                        AcceptUntrustedCertificates = true,
                        EnableNativeEvents = true
                    };
                    return new FirefoxDriver(firefoxProfile);
                }
                case DriverType.IE:
                {
                    var internetExplorerOptions = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        InitialBrowserUrl = "about:blank",
                        EnableNativeEvents = true,
                        IgnoreZoomLevel = true
                    };
                    return new InternetExplorerDriver(internetExplorerOptions);
                }
                case DriverType.Opera:
                {
                    var operaOptions = new OperaOptions();
                    operaOptions.AddArgument("--no-sandbox");
                    return new OperaDriver(operaOptions);
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