using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using Xunit;

namespace WebDriverManager.Tests
{
    public class LegacyChromeConfigTests : LegacyChromeConfig
    {
        [Fact]
        public void VersionTest()
        {
            var version = GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+.\d+.\d+$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
            Assert.Equal("114.0.5735.90", version);
        }

        [Fact]
        public void DriverDownloadLatestTest()
        {
            new DriverManager().SetUpDriver(new LegacyChromeConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void DriverDownloadExactTest()
        {
            new DriverManager().SetUpDriver(new LegacyChromeConfig(), "106.0.5249.61");
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }
    }
}
