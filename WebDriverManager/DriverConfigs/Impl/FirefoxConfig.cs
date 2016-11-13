using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class FirefoxConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Firefox";
        }

        public string GetUrl32()
        {
            return GetUrl64();
        }

        public string GetUrl64()
        {
            return
                "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win64.zip";
        }

        public string GetBinaryName()
        {
            return "geckodriver.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("https://github.com/mozilla/geckodriver/releases");
                doc.LoadHtml(htmlCode);
                var itemList =
                    doc.DocumentNode.SelectNodes("//*[@class='release-title']/a").Select(p => p.InnerText).ToList();
                var version = itemList.FirstOrDefault()?.Remove(0, 1);
                return version;
            }
        }
    }
}