namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class AppiumDriverManager : Logging, IBaseBrowserManager
    {
        private const string InstallationCommand = "/SP- /silent /noicons /closeapplications /dir=expand:%1";

        /// <summary>
        /// Set target appium driver architecture to x32 by default because of only 32 architecture presented
        /// </summary>
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "appium-installer.exe",
            Url = "https://bitbucket.org/appium/appium.app/downloads/AppiumForWindows_<version>.zip",
            PathVariable = "appium.binary.path",
            Architecture = Architecture.X32.ToString()
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://bitbucket.org/appium/appium.app/downloads");
                    doc.LoadHtml(htmlCode);
                    var itemList =
                        doc.DocumentNode.SelectNodes(
                                "//tr[@class='iterable-item']/td[@class='name']/a[contains(.,'AppiumForWindows_')]")
                            .Select(p => p.InnerText)
                            .ToList();
                    var item = itemList.FirstOrDefault();
                    var version = item?.Substring(item.IndexOf(item.Split('_')[1], StringComparison.Ordinal))
                        .Split('.')[0];
                    Log?.Info($"Latest appium driver version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last appium driver version");
                throw new WebDriverManagerException("Error occurred during getting last appium driver version", ex);
            }
        }

        public AppiumDriverManager()
        {
            _config.Version = GetLatestVersion();
        }

        public AppiumDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set appium driver version to: '{version}'");
        }

        public void Init()
        {
            _config.Destication = Path.Combine(Directory.GetCurrentDirectory(), _config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destication = destination;
            Log?.Info($"Set custom appium driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(_config);
            WebDriverManager.Unzip(_config);
            WebDriverManager.Clean();
            WebDriverManager.Install(InstallationCommand);
        }
    }
}