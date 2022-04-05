using System;
using System.Linq;
using System.Net;
using AngleSharp.Html.Parser;

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
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "operadriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://github.com/operasoftware/operachromiumdriver/releases");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll("[class='Link--primary']")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Trim(' ', '\r', '\n');
                return version;
            }
        }

        public virtual string GetMatchingBrowserVersion()
        {
            throw new NotImplementedException();
        }
    }
}
