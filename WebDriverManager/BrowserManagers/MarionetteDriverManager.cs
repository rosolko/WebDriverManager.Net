namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class MarionetteDriverManager : Logging, IBaseBrowserManager
    {
        /// <summary>
        /// Set target marionette driver architecture to x64 by default because of only 64 architecture presented
        /// Set binary name as pattern because of it's changing in accordance with marionette driver version
        /// </summary>
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "geckodriver.exe",
            Url = "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-win64.zip",
            PathVariable = "webdriver.gecko.driver",
            Architecture = Architecture.X64.ToString()
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://github.com/mozilla/geckodriver/releases");
                    doc.LoadHtml(htmlCode);
                    var itemList =
                        doc.DocumentNode.SelectNodes("//*[@class='release-title']/a").Select(p => p.InnerText).ToList();
                    var version = itemList.FirstOrDefault()?.Remove(0, 1);
                    Log?.Info($"Latest marionette driver version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last marionette driver version");
                throw new WebDriverManagerException("Error occurred during getting last marionette driver version", ex);
            }
        }

        public MarionetteDriverManager()
        {
            _config.Version = GetLatestVersion();
        }

        public MarionetteDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set marionette driver version to: '{version}'");
        }

        public void Init()
        {
            _config.Destication = Path.Combine(Directory.GetCurrentDirectory(), _config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destication = destination;
            Log?.Info($"Set custom marionette driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(_config);
            WebDriverManager.Unzip(_config);
            WebDriverManager.Clean();
            WebDriverManager.AddEnvironmentVariable(_config.PathVariable);
            WebDriverManager.UpdatePath(_config.PathVariable);
        }
    }
}