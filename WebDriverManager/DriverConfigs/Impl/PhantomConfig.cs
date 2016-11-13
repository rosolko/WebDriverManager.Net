using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class PhantomConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Phantom";
        }

        public string GetUrl32()
        {
            return "https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-<version>-windows.zip";
        }

        public string GetUrl64()
        {
            return GetUrl32();
        }

        public string GetBinaryName()
        {
            return "phantomjs.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("https://bitbucket.org/ariya/phantomjs/downloads");
                doc.LoadHtml(htmlCode);
                var itemList =
                    doc.DocumentNode.SelectNodes("//tr[@class='iterable-item']/td[@class='name']/a")
                        .Select(p => p.InnerText)
                        .ToList();
                var version = itemList.FirstOrDefault()?.Split('-')[1];
                return version;
            }
        }
    }
}