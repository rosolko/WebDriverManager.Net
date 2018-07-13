using System.Linq;
using System.Net;
using AngleSharp;
using AngleSharp.Parser.Html;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class PhantomConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Phantom";
        }

        public virtual string GetUrl32()
        {
            return "https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-<version>-windows.zip";
        }

        public virtual string GetUrl64()
        {
            return GetUrl32();
        }

        public virtual string GetBinaryName()
        {
            return "phantomjs.exe";
        }

        public virtual string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://bitbucket.org/ariya/phantomjs/downloads");
                var parser = new HtmlParser(Configuration.Default.WithDefaultLoader());
                var document = parser.Parse(htmlCode);
                var version = document.QuerySelectorAll(".iterable-item > .name > a")
                    .Select(element => element.TextContent)
                    .FirstOrDefault(item => !item.Contains("beta"));
                version = version?.Split('-')[1];
                return version;
            }
        }
    }
}
