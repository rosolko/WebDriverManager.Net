using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class EdgeConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Edge";
        }

        public string GetUrl32()
        {
            return GetUrl();
        }

        public string GetUrl64()
        {
            return GetUrl32();
        }

        public string GetBinaryName()
        {
            return "MicrosoftWebDriver.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode =
                    client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                doc.LoadHtml(htmlCode);
                var itemList = doc.DocumentNode.SelectNodes("//*[@class='driver-download']/p")
                    .Select(p => p.InnerText).ToList();
                var version = itemList.FirstOrDefault()?.Split(' ')[1].Split(' ')[0];
                return version;
            }
        }

        public static string GetUrl()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode =
                    client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                doc.LoadHtml(htmlCode);
                var itemList = doc.DocumentNode.SelectNodes("//*[@class='driver-download']/a")
                    .Select(p => p.GetAttributeValue("href", null)).ToList();
                var url = itemList.FirstOrDefault();
                return url;
            }
        }
    }
}