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
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "MicrosoftWebDriver.exe",
            Url = string.Empty,
            PathVariable = "webdriver.edge.driver",
            Architecture = Architecture.x32.ToString()
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode =
                        client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='driver-download']/p")
                        .Select(p => p.InnerText).ToList();
                    var version = itemList.FirstOrDefault()?.Split(' ')[1].Split(' ')[0];
                    Log?.Info($"Latest edge driver version is '{version}'");
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
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode =
                        client.DownloadString("https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='driver-download']/a")
                        .Select(p => p.GetAttributeValue("href", null)).ToList();
                    var url = itemList.FirstOrDefault();
                    Log?.Info($"Edge driver download url is '{url}'");
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
        {
            _config.Version = GetLatestVersion();
            _config.Url = GetDownloadUrl();
        }

        public void Init()
        {
            _config.Destination = Path.Combine(Directory.GetCurrentDirectory(), WebDriverManagerConfig.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destination = destination;
            Log?.Debug($"Set custom edge driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(_config);
            WebDriverManager.AddEnvironmentVariable(_config.PathVariable);
            WebDriverManager.UpdatePath(_config.PathVariable);
        }
    }
}