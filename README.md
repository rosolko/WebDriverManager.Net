[![Build status](https://ci.appveyor.com/api/projects/status/kjpqb5twmpxw6lpl?svg=true)](https://ci.appveyor.com/project/rosolko/webdrivermanager-net)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=rosolko_WebDriverManager.Net&metric=alert_status)](https://sonarcloud.io/dashboard?id=rosolko_WebDriverManager.Net)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=rosolko_WebDriverManager.Net&metric=coverage)](https://sonarcloud.io/dashboard?id=rosolko_WebDriverManager.Net)
[![NuGet](https://img.shields.io/nuget/v/WebDriverManager.svg)](https://www.nuget.org/packages/WebDriverManager)

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

Target is **netstandard2.0**.

After installation you can let WebDriverManager.Net to do manage WebDriver binaries for your application/test. Take a look to this NUnit example which uses Chrome with Selenium WebDriver:

```csharp
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
```

Notice that simply adding ``new DriverManager().SetUpDriver(<config>)`` does magic for you:

1. It checks the latest version of the WebDriver binary file
2. It downloads the binary WebDriver if it is not present in your system

So far, WebDriverManager supports **Chrome**, **Microsoft Edge**, **Firefox(Marionette)**, **Internet Explorer**, **Opera** or **PhantomJS** configs (Just change <config> to prefered config):

```csharp
new ChromeConfig();
new EdgeConfig();
new FirefoxConfig();
new InternetExplorerConfig();
new OperaConfig();
new PhantomConfig();
```

## Advanced

You can use WebDriverManager in two ways:
1. Automatic
2. Manual

#### Automatic way: 
```csharp
new DriverManager().SetUpDriver(new <Driver>Config());
```

You can also specify version:  
	``new DriverManager().SetUpDriver(new ChromeConfig(), "2.25")``

Or architecture:  
	``new DriverManager().SetUpDriver(new ChromeConfig(), "Latest", Architecture.X32)``

Or version and architecture:  
	``new DriverManager().SetUpDriver(new ChromeConfig(), "2.25", Architecture.X64)``

Only for Google Chrome so far, you can specify to automatically download a ```chromedriver.exe``` matching the version of the browser that is installed in your machine:  
    ``new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser); ``

#### Manual way:
```csharp
new DriverManager().SetUpDriver(
    "https://chromedriver.storage.googleapis.com/2.25/chromedriver_win32.zip",
    Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe"),
    "chromedriver.exe"
);
```

### If you want use your own implementation you need to create driver config and use it for set up(ex get, setup and work with phantomjs driver from taobao mirror):
```csharp
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
```

### Also you can implement your own services for download binaries and manage variables:
```csharp
public class CustomBinaryService : IBinaryService
{
    public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
    {
        ...
        // your implementation
        ...
    }
}

public class CustomVariableService : IVariableService
{
    public void SetupVariable(string path)
    {
        ...
        // your implementation
        ...
    }
}

...

new DriverManager(new CustomBinaryService(), new CustomVariableService()).SetUpDriver(new FirefoxConfig());
```

### Or you can modify existed drivers and change only necessary fields(same example):
```csharp
public class TaobaoPhantomConfig : PhantomConfig
{
    public override string GetName()
    {
        return "TaobaoPhantom";
    }

    public override string GetUrl32()
    {
        return "https://npm.taobao.org/mirrors/phantomjs/phantomjs-<version>-windows.zip";
    }
}

...

new DriverManager().SetUpDriver(new TaobaoPhantomConfig());
```

### Using with proxy:
```csharp
new DriverManager().WithProxy(previouslyInitializedProxy).SetUpDriver(new ChromeConfig());
```

## Thanks
Thanks to the following companies for generously providing their services/products to help improve this project:

 Logo | Description 
------------ | -------------
![AppVeyor](https://s3-us-west-2.amazonaws.com/svgporn.com/logos/appveyor.svg) | [AppVeyor](https://appveyor.com) is a hosted, distributed continuous integration service used to build and test projects hosted at GitHub on a Microsoft Windows virtual machine.
![BrowserStack](https://s3-us-west-2.amazonaws.com/svgporn.com/logos/browserstack.svg) | [BrowserStack](https://www.browserstack.com) is a cloud-based cross-browser testing tool that enables developers to test their websites across various browsers on different operating systems and mobile devices, without requiring users to install virtual machines, devices or emulators.
![GitHub](https://s3-us-west-2.amazonaws.com/svgporn.com/logos/github-icon.svg) | [GitHub](https://github.com) is a web-based Git repository hosting service. It offers all of the distributed version control and source code management (SCM) functionality of Git as well as adding several collaboration features such as bug tracking, feature requests, task management, and wikis for every project.
![JetBrains](https://s3-us-west-2.amazonaws.com/svgporn.com/logos/jetbrains.svg) | [JetBrains](https://www.jetbrains.com)  (formerly IntelliJ) is a software development company whose tools are targeted towards software developers and project managers.
![SonarQube](https://www.sonarqube.org/assets/logo-31ad3115b1b4b120f3d1efd63e6b13ac9f1f89437f0cf6881cc4d8b5603a52b4.svg) | [SonarQube](https://www.sonarqube.org) (formerly Sonar) is an open source platform for continuous inspection of code quality to perform automatic reviews with static analysis of code to detect bugs, code smells and security vulnerabilities on 20+ programming languages.

## About

WebDriverManager.Net (Copyright &copy; 2016-2021) is a personal project of [Aliaksandr Rasolka] licensed under [MIT] license. 
Comments, questions and suggestions are always very welcome!

[Aliaksandr Rasolka]: https://github.com/rosolko
[WebDriverManager.Net]: https://www.nuget.org/packages/WebDriverManager
[Selenium Webdriver]: http://docs.seleniumhq.org/projects/webdriver
[MIT]: https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE
