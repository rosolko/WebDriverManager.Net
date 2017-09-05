using WebDriverManager.DriverConfigs.Impl;

namespace IntegrationTests.DriverManagerTests
{
    public class TaobaoChromeConfig : ChromeConfig
    {
        public override string GetName()
        {
            return "TaobaoChrome";
        }

        public override string GetUrl32()
        {
            return "https://npm.taobao.org/mirrors/chromedriver/<version>/chromedriver_win32.zip";
        }
    }
}