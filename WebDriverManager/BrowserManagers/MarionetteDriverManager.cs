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
        /// Set target marionette driver architecture to x32 by default because of only 32 architecture presented
        /// Set binary name as pattern because of it's changing in accordance with marionette driver version
        /// </summary>
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "wires-<version>-win.exe",
            url = "https://github.com/jgraham/wires/releases/download/v<version>/wires-<version>-win.zip",
            pathVariable = "webdriver.gecko.driver",
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
                    var htmlCode = client.DownloadString("https://github.com/jgraham/wires/releases");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//*[@class='release-title']/a").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault();
                    if (version != null || version != string.Empty)
                    {
                        SetBinary(version);
                        Log?.Info($"Latest marionette driver version is '{version}'");
                    }
                    else
                        Log?.Warn($"Problem with getting latest marionette driver version. Parsed version is '{version}'");
                    return version;
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last marionette driver version");
                throw new WebDriverManagerException("Error occurred during getting last marionette driver version", ex);
            }
        }

        private void SetBinary(string version)
        {
            config.binary = config.binary.Replace("<version>", version);
            Log?.Trace($"Update binary name to: '{config.binary}' in accordance with marionette driver version: '{version}'");
        }

        public MarionetteDriverManager()
            : base()
        {
            config.version = GetLatestVersion();
        }

        public MarionetteDriverManager(string version)
            : base()
        {
            config.version = version;
            Log?.Info($"Set marionette driver version to: '{version}'");
            SetBinary(version);
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Info($"Set custom marionette driver destination path to: '{destination}'");
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
