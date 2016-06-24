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
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "IEDriverServer.exe",
            url = "http://selenium-release.storage.googleapis.com/<release>/IEDriverServer_<architecture>_<version>.zip",
            pathVariable = "webdriver.ie.driver",
            architecture = Architecture.x32.ToString().Replace("x", "Win")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string version = null;
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("http://www.seleniumhq.org/download");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("(//div[@id='mainContent']/p)[6]").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault().Split(' ')[2];
                    if (version != null || version != string.Empty)
                        Log?.Info($"Latest internet explorer driver version is '{version}'");
                    else
                        Log?.Warn($"Problem with getting latest internet explorer driver version. Parsed version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last internet explorer driver version");
                throw new WebDriverManagerException("Error occurred during getting last internet explorer driver version", ex);
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
                string release = null;
                release = version.Substring(0, version.LastIndexOf("."));
                if (release != null || release != string.Empty)
                    Log?.Debug($"Internet explorer driver release number is '{release}'");
                else
                    Log?.Warn($"Problem with getting internet explorer driver release number. Parsed release number is '{release}'");
                return release;
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last internet explorer driver release number");
                throw new WebDriverManagerException("Error occurred during getting last internet explorer driver release number", ex);
            }
        }

        public InternetExplorerDriverManager()
            : base()
        {
            config.version = GetLatestVersion();
            config.release = GetRelease(config.version);
        }

        public InternetExplorerDriverManager(string version)
            : base()
        {
            config.version = version;
            Log?.Info($"Set internet explorer driver version to: '{version}'");
            config.release = GetRelease(config.version);
        }

        public InternetExplorerDriverManager(Architecture architecture)
        {
            config.version = GetLatestVersion();
            config.release = GetRelease(config.version);
            SetArchitecture(architecture);
        }

        public InternetExplorerDriverManager(string version, Architecture architecture)
        {
            config.version = version;
            Log?.Info($"Set internet explorer driver version to: '{version}'");
            config.release = GetRelease(config.version);
            SetArchitecture(architecture);
        }

        private void SetArchitecture(Architecture architecture)
        {
            switch (architecture)
            {
                case Architecture.x32:
                    {
                        config.architecture = Architecture.x32.ToString().Replace("x", "Win");
                        break;
                    }
                case Architecture.x64:
                    {
                        config.architecture = Architecture.x64.ToString();
                        break;
                    }
                default:
                    break;
            }
            Log?.Info($"Set internet explorer driver architecture to: '{config.architecture}'");
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Info($"Set custom internet explorer driver destination path to: '{destination}'");
            Base();
        }

        public void Base()
        {
            WebDriverManager.Download(config);
            WebDriverManager.Unzip(config);
            WebDriverManager.Clean();
            WebDriverManager.AddEnvironmentVariable(config.pathVariable);
            // Temporary disable this functionality because of wrong path override
            //WebDriverManager.UpdatePath(config.pathVariable);
        }
    }
}
