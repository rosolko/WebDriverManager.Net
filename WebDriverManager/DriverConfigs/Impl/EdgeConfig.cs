using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Parser.Html;

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
                var htmlCode = client.DownloadString(
                    "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                var parser = new HtmlParser(Configuration.Default.WithDefaultLoader());
                var document = parser.Parse(htmlCode);
                var version = document
                    .QuerySelectorAll("[class='driver-download'] p")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Split(' ')[1]
                    .Split(' ')[0];
                return version;
            }
        }

        private static string GetUrl()
        {
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString(
                    "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                var parser = new HtmlParser(Configuration.Default.WithDefaultLoader());
                var document = parser.Parse(htmlCode);
                var url = document
                    .QuerySelectorAll("[class='driver-download'] a")
                    .Select(element => element.Attributes.GetNamedItem("href"))
                    .FirstOrDefault()
                    ?.Value;
                return url;
            }
        }
    }
}