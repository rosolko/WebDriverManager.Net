using System.Linq;
using System.Net;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class FirefoxConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Firefox";
        }

        public virtual string GetUrl32()
        {
            return
                "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win32.zip";
        }

        public virtual string GetUrl64()
        {
            return
                "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "geckodriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://github.com/mozilla/geckodriver/releases");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll(".release-header .f1 a")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Remove(0, 1);
                return version;
            }
        }
    }
}