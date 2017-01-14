using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace IntegrationTests
{
    public class VersionTests
    {
        [Fact]
        public void ChromeVersionResultNotEmptyAndMatch()
        {
            var chromeConfig = new ChromeConfig();
            var version = chromeConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }

        [Fact]
        public void EdgeVersionResultNotEmptyAndMatch()
        {
            var edgeConfig = new EdgeConfig();
            var version = edgeConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }

        [Fact]
        public void FirefoxVersionResultNotEmptyAndMatch()
        {
            var firefoxConfig = new FirefoxConfig();
            var version = firefoxConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }

        [Fact]
        public void IEVersionResultNotEmptyAndMatch()
        {
            var ieConfig = new IEConfig();
            var version = ieConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }

        [Fact]
        public void OperaVersionResultNotEmptyAndMatch()
        {
            var operaConfig = new OperaConfig();
            var version = operaConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }

        [Fact]
        public void PhantomVersionResultNotEmptyAndMatch()
        {
            var phantomConfig = new PhantomConfig();
            var version = phantomConfig.GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }
    }
}