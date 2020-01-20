using WebDriverManager.DriverConfigs.Impl;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 2)]
namespace WebDriverManager.Tests
{
    public class DriverManagerTests
    {
        [Fact]
        public void ChromeTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
        }
    }

    public class ParallelDriverManagerTests
    {
        [Fact]
        public void ParallelChromeTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
        }
    }
}
