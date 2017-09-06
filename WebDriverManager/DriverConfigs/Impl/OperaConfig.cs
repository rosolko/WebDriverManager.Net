using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Parser.Html;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class OperaConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Opera";
        }

        public virtual string GetUrl32()
        {
            return "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            return "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "operadriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://github.com/operasoftware/operachromiumdriver/releases");
                var parser = new HtmlParser(Configuration.Default.WithDefaultLoader());
                var document = parser.Parse(htmlCode);
                var version = document.QuerySelectorAll("[class='release-title'] a")
                    .Select(element => element.TextContent)
                    .FirstOrDefault();
                return version;
            }
        }
    }
}