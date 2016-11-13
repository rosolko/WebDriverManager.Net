[![AppVeyor](https://img.shields.io/appveyor/ci/rosolko/webdrivermanager-net.svg?maxAge=3600)](https://ci.appveyor.com/project/rosolko/webdrivermanager-net)
[![NuGet](https://img.shields.io/nuget/v/WebDriverManager.svg?maxAge=3600)](https://www.nuget.org/packages/WebDriverManager)
[![GitHub release](https://img.shields.io/github/release/rosolko/WebDriverManager.Net.svg?maxAge=3600)](https://github.com/rosolko/WebDriverManager.Net/releases/latest)
[![license](https://img.shields.io/github/license/rosolko/WebDriverManager.Net.svg?maxAge=3600)](https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE)

# WebDriverManager.Net
This small library aimed to automate the [Selenium WebDriver] binaries management inside a .Net project.

If you have ever used [Selenium WebDriver], you probably know that in order to use some browsers (for example **Chrome**) you need to download a binary which allows WebDriver to handle the browser. 
In addition, the absolute path to this binary must be set as part of the PATH environment variable or manually copied to build output folder (working directory).

This is quite annoying since it forces you to link directly this binary in your source code. In addition, you have to check manually when new versions of the binaries are released. This library comes to the rescue, performing in an automated way all this dirty job for you.

WebDriverManager is open source, released under the terms of [MIT] license.

## Installation

[WebDriverManager.Net] can be downloaded from NuGet.
Use the GUI or the following command in the Package Manager Console:

    PM> Install-Package WebDriverManager

## Usage

Then you can let WebDriverManager.Net to do manage WebDriver binaries for your application/test. Take a look to this NUnit example which uses Chrome with Selenium WebDriver:

    using NUnit.Framework;
	using OpenQA.Selenium;
	using OpenQA.Selenium.Chrome;
	using WebDriverManager;
	using WebDriverManager.DriverConfigs.Impl;

	namespace Test
	{
	    [TestFixture]
	    public class Tests
	    {
	        private IWebDriver _webDriver;

	        [SetUp]
	        public void SetUp()
	        {
	            new DriverManager().SetUpDriver(new ChromeConfig());
	            _webDriver = new ChromeDriver();
	        }

	        [TearDown]
	        public void TearDown()
	        {
	            _webDriver.Quit();
	        }

	        [Test]
	        public void Test()
	        {
	            _webDriver.Navigate().GoToUrl("https://www.google.com");
	            Assert.True(_webDriver.Title.Contains("Google"));
	        }
	    }
	}

Notice that simple adding ``new DriverManager().SetUpDriver(<config>)`` does magic for you:

1. It checks the latest version of the WebDriver binary file
2. It downloads the binary WebDriver if it is not present in your system

So far, WebDriverManager supports **Appium**, **Chrome**, **Microsoft Edge**, **Firefox(Marionette)**, **Internet Explorer**, **Opera** or **PhantomJS** configs (Just change <config> to prefered config):

    new AppiumConfig();
    new ChromeConfig();
    new EdgeConfig();
    new FirefoxConfig();
    new IEConfig();
    new OperaConfig();
    new PhantomConfig();

## Advanced

You can use WebDriverManager in two ways:
1. Automatic
2. Manual

#### Automatic way: 
	new DriverManager().SetUpDriver(new <Driver>Config());

You can also specify version:
	``new DriverManager().SetUpDriver(new ChromeConfig(), "2.25")``

Or architecture:
	``new DriverManager().SetUpDriver(new ChromeConfig(), "Latest", Architecture.X32)``

Or version and architecture:
	``new DriverManager().SetUpDriver(new ChromeConfig(), "2.25", Architecture.X64)``

#### Manual way:
	new DriverManager().SetUpDriver(
                "https://chromedriver.storage.googleapis.com/2.25/chromedriver_win32.zip", 
                Directory.GetCurrentDirectory(),
                "chromedriver.exe"
            );

### If you want use your own implementation you need to create driver config and use it for set up:
	public class CustomDriverConfig : IDriverConfig
    {
        public string GetName()
        {
            return "CustomDriverName";
        }

        public string GetUrl32()
        {
            return "https://someurl/<version>/win32.zip";
        }

        public string GetUrl64()
        {
            return "https://someurl/<version>/win64.zip";
        }

        public string GetBinaryName()
        {
            return "binary.name.exe";
        }

        public string GetLatestVersion()
        {
            <some code that get and return latest version>
        }
    }

    ...

    new DriverManager().SetUpDriver(new CustomDriverConfig());

## About

WebDriverManager.Net (Copyright &copy; 2016) is a personal project of [Alexander Rosolko] licensed under [MIT] license. 
Comments, questions and suggestions are always very welcome!

[Alexander Rosolko]: https://github.com/rosolko
[WebDriverManager.Net]: https://www.nuget.org/packages/WebDriverManager
[Selenium Webdriver]: http://docs.seleniumhq.org/projects/webdriver
[MIT]: https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE
