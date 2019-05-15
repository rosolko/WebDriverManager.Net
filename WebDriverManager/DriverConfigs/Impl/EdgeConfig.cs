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
            return "https://az813057.vo.msecnd.net/webdriver/msedgedriver_x86/msedgedriver.exe";
        }

        public virtual string GetUrl64()
        {
            return "https://az813057.vo.msecnd.net/webdriver/msedgedriver_x64/msedgedriver.exe";
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
                var version = document.QuerySelectorAll(".driver-download > a + p")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Split(' ')[1]
                    .Split(' ')[0];
                return version;
            }
        }
    }
}
