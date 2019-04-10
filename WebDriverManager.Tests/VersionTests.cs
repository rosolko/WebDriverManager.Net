using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class VersionData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {new ChromeConfig(), @"^\d+\.\d+.\d+.\d+$"},
            new object[] {new EdgeConfig(), @"^\d+\.\d+$"},
            new object[] {new FirefoxConfig(), @"^\d+\.\d+\.\d+$"},
            new object[] {new InternetExplorerConfig(), @"^\d+\.\d+\.\d+$"},
            new object[] {new OperaConfig(), @"^\d+\.\d+$"},
            new object[] {new PhantomConfig(), @"^\d+\.\d+\.\d+$"}
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class VersionTests
    {
        [Theory, ClassData(typeof(VersionData))]
        protected void VersionTest(IDriverConfig driverConfig, string pattern)
        {
            var version = driverConfig.GetLatestVersion();
            var regex = new Regex(pattern);
            var match = regex.Match(version);
            Assert.NotEmpty(version);
            Assert.True(match.Success);
        }
    }
}