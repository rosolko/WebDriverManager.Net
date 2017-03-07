[![AppVeyor](https://img.shields.io/appveyor/ci/rosolko/webdrivermanager-net.svg?maxAge=3600)](https://ci.appveyor.com/project/rosolko/webdrivermanager-net)
[![Quality Gate](https://sonarqube.com/api/badges/gate?key=rosolko:webdrivermanager)](https://sonarqube.com/dashboard/index/rosolko%3Awebdrivermanager)
[![NuGet](https://img.shields.io/nuget/v/WebDriverManager.svg?maxAge=3600)](https://www.nuget.org/packages/WebDriverManager)
[![GitHub release](https://img.shields.io/github/release/rosolko/WebDriverManager.Net.svg?maxAge=3600)](https://github.com/rosolko/WebDriverManager.Net/releases/latest)
[![license](https://img.shields.io/github/license/rosolko/WebDriverManager.Net.svg?maxAge=3600)](https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE)

# WebDriverManager.Net

## Table of contents

  * [Info](#info)
  * [Installation](#installation)
  * [Usage](#usage)
  * [Advanced](#advanced)
  * [Thanks](#thanks)
  * [About](#about)

## Info
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

So far, WebDriverManager supports **Chrome**, **Microsoft Edge**, **Firefox(Marionette)**, **Internet Explorer**, **Opera** or **PhantomJS** configs (Just change <config> to prefered config):

    new ChromeConfig();
    new EdgeConfig();
    new FirefoxConfig();
    new InternetExplorerConfig();
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

### If you want use your own implementation you need to create driver config and use it for set up(ex get, setup and wotk with phantomjs driver from taobao mirror):
	public class TaobaoPhantomConfig : IDriverConfig
    {
        public string GetName()
        {
            return "TaobaoPhantom";
        }

        public string GetUrl32()
        {
            return "https://npm.taobao.org/mirrors/phantomjs/phantomjs-<version>-windows.zip";
        }

        public string GetUrl64()
        {
            return GetUrl32();
        }

        public string GetBinaryName()
        {
            return "phantomjs.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("https://bitbucket.org/ariya/phantomjs/downloads");
                doc.LoadHtml(htmlCode);
                var itemList =
                    doc.DocumentNode.SelectNodes("//tr[@class='iterable-item']/td[@class='name']/a")
                        .Select(p => p.InnerText)
                        .ToList();
                var version = itemList.FirstOrDefault()?.Split('-')[1];
                return version;
            }
        }
    }

    ...

    new DriverManager().SetUpDriver(new TaobaoPhantomConfig());

### Or you can modify existed drivers and change only necessary fields(same example):
	public class TaobaoPhantomConfig : PhantomConfig
    {
        public string GetName()
        {
            return "TaobaoPhantom";
        }

        public string GetUrl32()
        {
            return "https://npm.taobao.org/mirrors/phantomjs/phantomjs-<version>-windows.zip";
        }
    }

    ...

    new DriverManager().SetUpDriver(new TaobaoPhantomConfig());

## Thanks
Thanks to the following companies for generously providing their services/products to help improve this project:

 Logo | Description 
------------ | -------------
 ![BrowserStack](https://cdn.rawgit.com/rosolko/5df6447289fb4f40f14311d0275e7869/raw/01758b5090bf6a95710d703891692567234bfc25/browserstack.svg) | [BrowserStack](https://www.browserstack.com) is a cloud-based cross-browser testing tool that enables developers to test their websites across various browsers on different operating systems and mobile devices, without requiring users to install virtual machines, devices or emulators.
![GitHub](https://cdn.rawgit.com/rosolko/3ab6cb55f21009a2bc8eb1749170958e/raw/bf0cf26772bea67340c4a600b5339b25839e0696/github.svg) | [GitHub](https://github.com) is a web-based Git repository hosting service. It offers all of the distributed version control and source code management (SCM) functionality of Git as well as adding several collaboration features such as bug tracking, feature requests, task management, and wikis for every project.
![AppVeyor](https://cdn.rawgit.com/rosolko/97d00e90fa68763bbd28462b17bda903/raw/71a23a11b4efdafbc31356e19cbbb1c155ff62fb/appveyor.svg) | [AppVeyor](https://appveyor.com) is a hosted, distributed continuous integration service used to build and test projects hosted at GitHub on a Microsoft Windows virtual machine.

## About

WebDriverManager.Net (Copyright &copy; 2016-2017) is a personal project of [Alexander Rosolko] licensed under [MIT] license. 
Comments, questions and suggestions are always very welcome!

[Alexander Rosolko]: https://github.com/rosolko
[WebDriverManager.Net]: https://www.nuget.org/packages/WebDriverManager
[Selenium Webdriver]: http://docs.seleniumhq.org/projects/webdriver
[MIT]: https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE
