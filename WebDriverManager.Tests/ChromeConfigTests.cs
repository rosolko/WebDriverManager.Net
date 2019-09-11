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
        public void DriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void DriverOldReleaseDownloadTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), "LATEST_RELEASE_76");
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void DriverOldReleaseDownloadTest2()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), "LATEST_RELEASE_72.0.3626");
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }
    }
}
