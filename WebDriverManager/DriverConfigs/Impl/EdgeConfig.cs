using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class EdgeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://msedgedriver.azureedge.net/<version>/";

        public virtual string GetName()
        {
            return "Edge";
        }

        public virtual string GetUrl32()
        {
            return $"{BaseVersionPatternUrl}edgedriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"{BaseVersionPatternUrl}edgedriver_mac64.zip";
            }
            return $"{BaseVersionPatternUrl}edgedriver_win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "msedgedriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var htmlCode =
                    client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll(".driver-download > p.driver-download__meta")
                    .Select(element => element.TextContent).ToList()[2]
                    ?.Split(' ')[1]
                    .Split(' ')[0];
                return version;
            }
        }
    }
}
