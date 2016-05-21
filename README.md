# WebDriverManager.Net
Automatic Selenium WebDriver binaries management for .Net

Original application idea is going from - [Boni García].
Java implementation you can find here - [GitHub Repository].

This piece of software is a small library aimed to automate the [Selenium WebDriver] binaries management inside a .Net project.

If you have ever used [Selenium WebDriver], you probably know that in order to use some browsers (for example **Chrome**, **Internet Explorer**, **Opera**, **Microsoft Edge**, **PhantomJS**, or **Marionette**) you need to download a binary which allows WebDriver to handle the browser. 
In addition, the absolute path to this binary must be set as part of the PATH environment variable or manually copied to build output folder (working directory).

This is quite annoying since it forces you to link directly this binary in your source code. In addition, you have to check manually when new versions of the binaries are released. This library comes to the rescue, performing in an automated way all this dirty job for you.

WebDriverManager is open source, released under the terms of [MIT] license.

## Installation

[WebDriverManager.Net] can be downloaded from NuGet.
Use the GUI or the following command in the Package Manager Console:

    PM> Install-Package WebDriverManager.Net

## Usage

Then you can let WebDriverManager.Net to do manage WebDriver binaries for your application/test. Take a look to this NUnit example which uses Chrome with Selenium WebDriver:

    namespace BrowserTests
    {
        using OpenQA.Selenium;
        using OpenQA.Selenium.Chrome;
        using WebDriverManager.BrowserManagers;

        [TestFixture]
        public class ChromeTest 
        {
            protected IWebDriver driver;

            [TestFixtureSetUp]
            public void FixtureSetUp() 
            {
                new ChromeDriverManager().Init();
            }

            [SetUp]
            public void TestSetUp() 
            {
                driver = new ChromeDriver();
            }

            [TestFixtureTearDown]
            public void teardown() 
            {
                if (driver != null)
                    driver.Quit();
            }

            [Test]
            public void Test() {
                // Using Selenium WebDriver to carry out automated web testing
            }
        }
    }

Notice that simple adding ``new ChromeDriverManager().Init();`` WebDriverManager does magic for you:

1. It checks the latest version of the WebDriver binary file
2. It downloads the binary WebDriver if it is not present in your system

So far, WebDriverManager supports **Chrome**, **Microsoft Edge**, **Internet Explorer**, **Marionette**, **Opera** or **PhantomJS**  as follows:

    new ChromeDriverManager().Init();
    new EdgeDriverManager().Init();
    new InternetExplorerDriverManager().Init();
    new MarionetteDriverManager().Init();
    new OperaDriverManager().Init();
    new PhantomJsDriverManager().Init();

## Advanced

Configuration parameters for WebDriverManager are set in the class **constructor** parameter or **init** method parameter.

1. Target version can be specified in class constructor using parameter:

    ``new ChromeDriverManager("2.21").Init();``

    In this case manager try to download ChromeDriver binary with **2.11** version.

2. Also you can specify target binary architecture, for this case you need to include reference and specify architecture in class constructor using paramet
 
    ``using WebDriverManager.Helpers;``

    ``...``

    `` new InternetExplorerDriverManager(Architecture.x32).Init();``

    In this case manager try to download ChromeDriver binary with **x32** architecture.

3. Target driver binary destination folder can be specified in class constructor using parameter:
    
    ``new ChromeDriverManager().Init(@"C:\Binaries");``

    In this case manager try to download latest version of ChromeDriver and put binary to **C:\Binaries** folder.

NOTE 1: You can mix parameters as you want but *NOTE 2*.

NOTE 2: Some driver manager doesn't support vesion or architecture managements.

## About

WebDriverManager.Net (Copyright &copy; 2016) is a personal project of [Alexander Rosolko] licensed under [MIT] license. 
Comments, questions and suggestions are always very welcome!

[Alexander Rosolko]: https://github.com/rosolko
[WebDriverManager.Net]: https://www.nuget.org/packages
[Boni García]: http://bonigarcia.github.io
[GitHub Repository]: https://github.com/bonigarcia/webdrivermanager
[Selenium Webdriver]: http://docs.seleniumhq.org/projects/webdriver
[MIT]: https://github.com/rosolko/WebDriverManager.Net/blob/master/LICENSE
