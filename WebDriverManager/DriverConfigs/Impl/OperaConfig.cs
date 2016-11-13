using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class OperaConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Opera";
        }

        public string GetUrl32()
        {
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v<version>/operadriver_win32.zip";
        }

        public string GetUrl64()
        {
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v<version>/operadriver_win64.zip";
        }

        public string GetBinaryName()
        {
            return "operadriver.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("https://github.com/operasoftware/operachromiumdriver/releases");
                doc.LoadHtml(htmlCode);
                var itemList = doc.DocumentNode.SelectNodes("//*[@class='release-title']/a")
                    .Select(p => p.InnerText).ToList();
                var version = itemList.FirstOrDefault();
                return version;
            }
        }
    }
}