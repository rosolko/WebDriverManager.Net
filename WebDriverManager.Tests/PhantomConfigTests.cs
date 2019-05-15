using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class PhantomConfigTests : PhantomConfig
    {
        [Fact]
        public void VersionTest()
        {
            var version = GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
        }

        [Fact]
        public void DriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new PhantomConfig());
        }
    }
}
