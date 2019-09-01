using System.Reflection;
using OpenQA.Selenium;

namespace WebDriverManager.Tests
{
    public static class WebDriverFinder
    {
        public static string FindFile(string fileName)
        {
            var type = typeof(IWebDriver).Assembly.GetType("OpenQA.Selenium.Internal.FileUtilities");
            var bindings = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
            return type.InvokeMember("FindFile", bindings, null, null, new object[] { fileName }).ToString();
        }
    }
}
