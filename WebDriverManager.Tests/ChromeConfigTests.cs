using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
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
        public void DriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }
    }
}
