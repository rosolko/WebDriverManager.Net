using System.Linq;
using System.Net;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class EdgeConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Edge";
        }

        public virtual string GetUrl32()
        {
            return GetUrl();
        }

        public virtual string GetUrl64()
        {
            return GetUrl32();
        }

        public virtual string GetBinaryName()
        {
            return "MicrosoftWebDriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var htmlCode =
                    client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll(".driver-download > a + p")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Split(' ')[1]
                    .Split(' ')[0];
                return version;
            }
        }

        public virtual string GetUrl()
        {
            using (var client = new WebClient())
            {
                var htmlCode =
                    client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var url = document.QuerySelectorAll(".driver-download > a")
                    .Select(element => element.Attributes.GetNamedItem("href"))
                    .FirstOrDefault()
                    ?.Value;
                return url;
            }
        }
    }
}