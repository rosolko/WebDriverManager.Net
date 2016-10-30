namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class PhantomJsDriverManager : Logging, IBaseBrowserManager
    {
        /// <summary>
        /// Set target phantomjs driver architecture to x32 by default because of only 32 architecture presented
        /// </summary>
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "phantomjs.exe",
            Url = "https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-<version>-windows.zip",
            PathVariable = "phantomjs.binary.path",
            Architecture = Architecture.X32.ToString().Replace("x", "")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://bitbucket.org/ariya/phantomjs/downloads");
                    doc.LoadHtml(htmlCode);
                    var itemList =
                        doc.DocumentNode.SelectNodes("//tr[@class='iterable-item']/td[@class='name']/a")
                            .Select(p => p.InnerText)
                            .ToList();
                    var version = itemList.FirstOrDefault()?.Split('-')[1];
                    Log?.Info($"Latest phantomjs driver version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last phantomjs driver version");
                throw new WebDriverManagerException("Error occurred during getting last phantomjs driver version", ex);
            }
        }

        public PhantomJsDriverManager()
        {
            _config.Version = GetLatestVersion();
        }

        public PhantomJsDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set phantomjs driver version to: '{version}'");
        }

        public void Init()
        {
            _config.Destication = Path.Combine(Directory.GetCurrentDirectory(), WebDriverManagerConfig.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destication = destination;
            Log?.Info($"Set custom phantomjs driver destination path to: '{destination}'");
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