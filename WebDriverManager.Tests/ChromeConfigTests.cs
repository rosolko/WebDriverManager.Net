using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class ChromeConfigTests : ChromeConfig
    {
        [Fact]
        public void VersionTest()
        {
            var version = GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+.\d+.\d+$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
        }

        [Fact]
        public void DriverDownloadLatestTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void DriverDownloadFromChromeStorageTest()
        {
            // Oldest stored version from https://chromedriver.storage.googleapis.com/index.html?path=73.0.3683.68/
            var oldVersion = "73.0.3683.68";
            new DriverManager().SetUpDriver(new ChromeConfig(), oldVersion);
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }
    }
}
