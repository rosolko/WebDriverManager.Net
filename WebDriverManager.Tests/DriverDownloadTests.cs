using System.Collections;
using System.Collections.Generic;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

namespace WebDriverManager.Tests
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
        [Theory, ClassData(typeof(DriverData))]
        protected void DriverDownloadTest(IDriverConfig driverConfig)
        {
            new DriverManager().SetUpDriver(driverConfig);
        }
    }
}