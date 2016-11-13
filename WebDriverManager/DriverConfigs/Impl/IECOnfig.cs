using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class IEConfig : IDriverConfig
    {
        public string GetName()
        {
            return "IE";
        }

        public string GetUrl32()
        {
            return "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_Win32_<version>.zip";
        }

        public string GetUrl64()
        {
            return "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_x64_<version>.zip";
        }

        public string GetBinaryName()
        {
            return "IEDriverServer.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("http://www.seleniumhq.org/download");
                doc.LoadHtml(htmlCode);
                var itemList = doc.DocumentNode.SelectNodes("(//div[@id='mainContent']/p)[7]")
                    .Select(p => p.InnerText).ToList();
                var version = itemList.FirstOrDefault()?.Split(' ')[2];
                return version;
            }
        }
    }
}