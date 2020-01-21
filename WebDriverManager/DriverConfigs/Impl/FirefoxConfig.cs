using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;
using Architecture = WebDriverManager.Helpers.Architecture;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class FirefoxConfig : IDriverConfig
    {
        private const string DownloadUrl = "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-";

        public virtual string GetName()
        {
            return "Firefox";
        }

        public virtual string GetUrl32()
        {
            return GetUrl(Architecture.X32);
        }

        public virtual string GetUrl64()
        {
            return GetUrl(Architecture.X64);
        }

        public virtual string GetBinaryName()
        {
            return "geckodriver" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
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

        private static string GetUrl(Architecture architecture)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"{DownloadUrl}macos.tar.gz";
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? $"{DownloadUrl}linux{((int)architecture).ToString()}.tar.gz"
                : $"{DownloadUrl}win{((int)architecture).ToString()}.zip";
        }
    }
}
