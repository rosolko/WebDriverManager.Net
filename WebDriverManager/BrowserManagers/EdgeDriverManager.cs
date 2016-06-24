namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class EdgeDriverManager : Logging, IBaseBrowserManager
    {
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "MicrosoftWebDriver.exe",
            url = "https://download.microsoft.com/download/1/4/1/14156DA0-D40F-460A-B14D-1B264CA081A5/MicrosoftWebDriver.exe",
            pathVariable = "webdriver.edge.driver",
            architecture = Architecture.x32.ToString()
        };

        public string GetLatestVersion()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string version = null;
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://www.microsoft.com/en-us/download/details.aspx?id=48740");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='fileinfo']//p").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault();
                    if (version != null || version != string.Empty)
                        Log?.Info($"Latest edge driver version is '{version}'");
                    else
                        Log?.Warn($"Problem with getting latest edge driver version. Parsed version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error("Error occurred during getting last edge driver version");
                throw new WebDriverManagerException("Error occurred during getting last edge driver version", ex);
            }
        }

        public EdgeDriverManager()
            : base()
        {
            config.version = GetLatestVersion();
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Debug($"Set custom edge driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(config);
            WebDriverManager.AddEnvironmentVariable(config.pathVariable);
            // Temporary disable this functionality because of wrong path override
            //WebDriverManager.UpdatePath(config.pathVariable);
        }
    }
}
