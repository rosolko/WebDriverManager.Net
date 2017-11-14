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
                    var options = new ChromeOptions();
                    options.AddArguments("--no-sandbox", "--disable-infobars", "--disable-save-password-bubble");
                    return new ChromeDriver(options);
                }
                case DriverType.Edge:
                {
                    var options = new EdgeOptions
                    {
                        PageLoadStrategy = PageLoadStrategy.Eager
                    };
                    return new EdgeDriver(options);
                }
                case DriverType.Firefox:
                {
                    var options = new FirefoxOptions
                    {
                        AcceptInsecureCertificates = true
                    };
                    return new FirefoxDriver(options);
                }
                case DriverType.InternetExplorer:
                {
                    var options = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        InitialBrowserUrl = "about:blank",
                        EnableNativeEvents = true,
                        IgnoreZoomLevel = true
                    };
                    return new InternetExplorerDriver(options);
                }
                case DriverType.Opera:
                {
                    var options = new OperaOptions();
                    options.AddArguments("--no-sandbox", "--disable-infobars", "--disable-save-password-bubble");
                    options.BinaryLocation = @"C:\Program Files\Opera\launcher.exe";
                    return new OperaDriver(options);
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