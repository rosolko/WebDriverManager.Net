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
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "operadriver.exe",
            url = "https://github.com/operasoftware/operachromiumdriver/releases/download/v<version>/operadriver_<architecture>.zip",
            pathVariable = "webdriver.opera.driver",
            architecture = Architecture.x32.ToString().Replace("x", "win")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string version = null;
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://github.com/operasoftware/operachromiumdriver/releases");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='release-title']/a").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault();
                    if (version != null || version != string.Empty)
                        Log?.Info($"Latest opera driver version is '{version}'");
                    else
                        Log?.Warn($"Problem with getting latest opera driver version. Parsed version is '{version}'");
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
            : base()
        {
            config.version = GetLatestVersion();
        }

        public OperaDriverManager(string version)
            : base()
        {
            config.version = version;
            Log?.Info($"Set opera driver version to: '{version}'");
        }

        public OperaDriverManager(Architecture architecture)
        {
            config.version = GetLatestVersion();
            SetArchitecture(architecture);
        }

        public OperaDriverManager(string version, Architecture architecture)
        {
            config.version = version;
            Log?.Info($"Set opera driver version to: '{version}'");
            SetArchitecture(architecture);
        }

        private void SetArchitecture(Architecture architecture)
        {
            switch (architecture)
            {
                case Architecture.x32:
                    {
                        config.architecture = Architecture.x32.ToString().Replace("x", "win");
                        break;
                    }
                case Architecture.x64:
                    {
                        config.architecture = Architecture.x64.ToString().Replace("x", "win");
                        break;
                    }
                default:
                    break;
            }
            Log?.Info($"Set opera driver architecture to: '{config.architecture}'");
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Info($"Set custom opera driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(config);
            WebDriverManager.Unzip(config);
            WebDriverManager.Clean();
            WebDriverManager.AddEnvironmentVariable(config.pathVariable);
            WebDriverManager.UpdatePath(config.pathVariable);
        }
    }
}
