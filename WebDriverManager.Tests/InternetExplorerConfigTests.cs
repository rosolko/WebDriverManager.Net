using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class InternetExplorerConfigTests : InternetExplorerConfig
    {
        [Fact]
        public void VersionTest()
        {
            var version = GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
        }

        [Fact(Skip = "The remote server returned an error: (404) Not Found")]
        public void DriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new InternetExplorerConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void GetMatchingBrowserVersionTest()
        {
            var version = GetMatchingBrowserVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+(\.\d+)?$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
        }
    }
}
