namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class InternetExplorerDriverManager : Logging, IBaseBrowserManager
    {
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "IEDriverServer.exe",
            Url = "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_<architecture>_<version>.zip",
            PathVariable = "webdriver.ie.driver",
            Architecture = Architecture.X32.ToString().Replace("x", "Win")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("http://www.seleniumhq.org/download");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("(//div[@id='mainContent']/p)[6]")
                        .Select(p => p.InnerText).ToList();
                    var version = itemList.FirstOrDefault()?.Split(' ')[2];
                    Log?.Info($"Latest internet explorer driver version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last internet explorer driver version");
                throw new WebDriverManagerException(
                    "Error occurred during getting last internet explorer driver version", ex);
            }
        }

        /// <summary>
        /// Get internet explorer driver release number from driver version. Because of driver version not always equals to release number
        /// </summary>
        /// <param name="version">Driver version</param>
        /// <returns>Release number</returns>
        private string GetRelease(string version)
        {
            try
            {
                var release = version.Substring(0, version.LastIndexOf(".", StringComparison.Ordinal));
                Log?.Debug($"Internet explorer driver release number is '{release}'");
                return release;
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last internet explorer driver release number");
                throw new WebDriverManagerException(
                    "Error occurred during getting last internet explorer driver release number", ex);
            }
        }

        public InternetExplorerDriverManager()
        {
            _config.Version = GetLatestVersion();
            _config.Release = GetRelease(_config.Version);
        }

        public InternetExplorerDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set internet explorer driver version to: '{version}'");
            _config.Release = GetRelease(_config.Version);
        }

        public InternetExplorerDriverManager(Architecture architecture)
        {
            _config.Version = GetLatestVersion();
            _config.Release = GetRelease(_config.Version);
            SetArchitecture(architecture);
        }

        public InternetExplorerDriverManager(string version, Architecture architecture)
        {
            _config.Version = version;
            Log?.Info($"Set internet explorer driver version to: '{version}'");
            _config.Release = GetRelease(_config.Version);
            SetArchitecture(architecture);
        }

        private void SetArchitecture(Architecture architecture)
        {
            switch (architecture)
            {
                case Architecture.X32:
                {
                    _config.Architecture = Architecture.X32.ToString().Replace("x", "Win");
                    break;
                }
                case Architecture.X64:
                {
                    _config.Architecture = Architecture.X64.ToString();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(architecture), architecture,
                        "Can't recognize architecture");
            }
            Log?.Info($"Set internet explorer driver architecture to: '{_config.Architecture}'");
        }

        public void Init()
        {
            _config.Destication = Path.Combine(Directory.GetCurrentDirectory(), WebDriverManagerConfig.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destication = destination;
            Log?.Info($"Set custom internet explorer driver destination path to: '{destination}'");
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