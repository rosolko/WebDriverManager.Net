namespace WebDriverManager.BrowserManagers
{
    using HtmlAgilityPack;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Helpers;

    public class OperaDriverManager : Logging, IBaseBrowserManager
    {
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "operadriver.exe",
            Url =
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v<version>/operadriver_<architecture>.zip",
            PathVariable = "webdriver.opera.driver",
            Architecture = Architecture.x32.ToString().Replace("x", "win")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://github.com/operasoftware/operachromiumdriver/releases");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='release-title']/a")
                        .Select(p => p.InnerText).ToList();
                    var version = itemList.FirstOrDefault();
                    Log?.Info($"Latest opera driver version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last opera driver version");
                throw new WebDriverManagerException("Error occurred during getting last opera driver version", ex);
            }
        }

        public OperaDriverManager()
        {
            _config.Version = GetLatestVersion();
        }

        public OperaDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set opera driver version to: '{version}'");
        }

        public OperaDriverManager(Architecture architecture)
        {
            _config.Version = GetLatestVersion();
            SetArchitecture(architecture);
        }

        public OperaDriverManager(string version, Architecture architecture)
        {
            _config.Version = version;
            Log?.Info($"Set opera driver version to: '{version}'");
            SetArchitecture(architecture);
        }

        private void SetArchitecture(Architecture architecture)
        {
            switch (architecture)
            {
                case Architecture.x32:
                {
                    _config.Architecture = Architecture.x32.ToString().Replace("x", "win");
                    break;
                }
                case Architecture.x64:
                {
                    _config.Architecture = Architecture.x64.ToString().Replace("x", "win");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(architecture), architecture,
                        "Can't recognize architecture");
            }
            Log?.Info($"Set opera driver architecture to: '{_config.Architecture}'");
        }

        public void Init()
        {
            _config.Destination = Path.Combine(Directory.GetCurrentDirectory(), WebDriverManagerConfig.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            _config.Destination = destination;
            Log?.Info($"Set custom opera driver destination path to: '{destination}'");
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