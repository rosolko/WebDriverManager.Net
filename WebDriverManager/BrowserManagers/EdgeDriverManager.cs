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
            url = string.Empty,
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
                    var htmlCode = client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'subsection__body')]//p[2]").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault().Split(' ')[1].Split(' ')[0];
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

        public string GetDownloadUrl()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string url = null;
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'subsection__body')]//p[1]/a").Select(p => p.GetAttributeValue("href", null)).ToList();
                    url = itemList.FirstOrDefault();
                    if (url != null || url != string.Empty)
                        Log?.Info($"Edge driver download url is '{url}'");
                    else
                        Log?.Warn($"Problem with getting edge driver download url. Parsed url is '{url}'");
                    return url;
                }
            }
            catch (Exception ex)
            {
                Log?.Error("Error occurred during getting last edge driver download url");
                throw new WebDriverManagerException("Error occurred during getting last edge driver download url", ex);
            }
        }

        public EdgeDriverManager()
            : base()
        {
            config.version = GetLatestVersion();
            config.url = GetDownloadUrl();
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
            WebDriverManager.UpdatePath(config.pathVariable);
        }
    }
}
