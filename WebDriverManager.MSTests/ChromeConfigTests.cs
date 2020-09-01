using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverManager.DriverConfigs.Impl;

namespace WebDriverManager.MSTests
{
    [TestClass]
    public class ChromeConfigTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), version: "matching_installed");
        }
    }
}
