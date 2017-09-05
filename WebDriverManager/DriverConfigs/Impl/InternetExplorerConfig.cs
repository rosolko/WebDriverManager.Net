using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Parser.Html;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class InternetExplorerConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "InternetExplorer";
        }

        public virtual string GetUrl32()
        {
            return "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_Win32_<version>.zip";
        }

        public virtual string GetUrl64()
        {
            return "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_x64_<version>.zip";
        }

        public virtual string GetBinaryName()
        {
            return "IEDriverServer.exe";
        }

        public virtual string GetLatestVersion()
        {
            var regex = new Regex(@"^\d+\.\d+\.\d+$");
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("http://www.seleniumhq.org/download");
                var parser = new HtmlParser(Configuration.Default.WithDefaultLoader());
                var document = parser.Parse(htmlCode);
                var version = document.QuerySelectorAll("#mainContent > p:nth-child(11)")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Split(' ')[2];
                return version != null && regex.Match(version).Success
                    ? version
                    : $"{version}.0";
            }
        }
    }
}