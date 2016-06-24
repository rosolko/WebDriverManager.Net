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
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "phantomjs.exe",
            url = "https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-<version>-windows.zip",
            pathVariable = "phantomjs.binary.path",
            architecture = Architecture.x32.ToString().Replace("x", "")
        };

        public string GetLatestVersion()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string version = null;
                    var doc = new HtmlDocument();
                    var htmlCode = client.DownloadString("https://bitbucket.org/ariya/phantomjs/downloads");
                    doc.LoadHtml(htmlCode);
                    var itemList = doc.DocumentNode.SelectNodes("//tr[@class='iterable-item']/td[@class='name']/a").Select(p => p.InnerText).ToList();
                    version = itemList.FirstOrDefault().Split('-')[1];
                    if (version != null || version != string.Empty)
                        Log?.Info($"Latest phantomjs driver version is '{version}'");
                    else
                        Log?.Warn($"Problem with getting latest phantomjs driver version. Parsed version is '{version}'");
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
            : base()
        {
            config.version = GetLatestVersion();
        }

        public PhantomJsDriverManager(string version)
            : base()
        {
            config.version = version;
            Log?.Info($"Set phantomjs driver version to: '{version}'");
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Info($"Set custom phantomjs driver destination path to: '{destination}'");
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
