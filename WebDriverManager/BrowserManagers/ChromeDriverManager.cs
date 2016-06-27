namespace WebDriverManager.BrowserManagers
{
    using System;
    using System.IO;
    using System.Net;
    using Helpers;

    public class ChromeDriverManager : Logging, IBaseBrowserManager
    {
        /// <summary>
        /// Set target chrome driver architecture to x32 by default because of only 32 architecture presented
        /// </summary>
        WebDriverManagerConfig config = new WebDriverManagerConfig
        {
            binary = "chromedriver.exe",
            url = "https://chromedriver.storage.googleapis.com/<version>/chromedriver_win<architecture>.zip",
            pathVariable = "webdriver.chrome.driver",
            architecture = Architecture.x32.ToString().Replace("x", "")
        };

        public string GetLatestVersion()
        {
            try
            {
                string version = null;
                var webRequest = WebRequest.Create(@"https://chromedriver.storage.googleapis.com/LATEST_RELEASE");

                using (var response = webRequest.GetResponse())
                {
                    using (var content = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(content))
                        {
                            version = reader.ReadToEnd().Trim();
                            if (version != null || version != string.Empty)
                                Log?.Info($"Latest chrome driver version is '{version}'");
                            else
                                Log?.Warn($"Problem with getting latest chrome driver version. Parsed version is '{version}'");
                            return version;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log?.Error(ex, "Error occurred during getting last chrome driver version");
                throw new WebDriverManagerException("Error occurred during getting last chrome driver version", ex);
            }
        }

        public ChromeDriverManager()
            : base()
        {
            config.version = GetLatestVersion();
        }

        public ChromeDriverManager(string version)
            : base()
        {
            config.version = version;
            Log?.Info($"Set chrome driver version to: '{version}'");
        }

        public void Init()
        {
            config.destication = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultDestinationFolder);
            Log?.Debug($"Use default chrome driver destination path: '{config.destication}'");
            Base();
        }

        public void Init(string destination)
        {
            config.destication = destination;
            Log?.Info($"Set custom chrome driver destination path to: '{destination}'");
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
