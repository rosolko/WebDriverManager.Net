using System.Collections;
using System.Collections.Generic;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace IntegrationTests
{
    public class DriverData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {new ChromeConfig()},
            new object[] {new EdgeConfig()},
            new object[] {new FirefoxConfig()},
            new object[] {new InternetExplorerConfig()},
            new object[] {new OperaConfig()},
            new object[] {new PhantomConfig()}
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

    public class DriverDownloadTests
    {
        [Theory, ClassData(typeof(DriverData)), Trait("Category", "Config")]
        protected void DriverDownloadTest(IDriverConfig driverConfig)
        {
            new DriverManager().SetUpDriver(driverConfig);
        }
    }
}