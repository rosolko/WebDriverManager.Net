using System;
using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class OperaConfigTests : OperaConfig
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
            new DriverManager().SetUpDriver(new OperaConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void GetMatchingBrowserVersionTest()
        {
            Assert.Throws<NotImplementedException>(GetMatchingBrowserVersion);
        }
    }
}
