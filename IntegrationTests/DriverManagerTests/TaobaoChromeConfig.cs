using WebDriverManager.DriverConfigs.Impl;

namespace IntegrationTests.DriverManagerTests
{
    public class TaobaoChromeConfig : ChromeConfig
    {
        public new string GetName()
        {
            return "TaobaoChrome";
        }

        public new string GetUrl32()
        {
            return "https://npm.taobao.org/mirrors/chromedriver/<version>/chromedriver_win32.zip";
        }
    }
}