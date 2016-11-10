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
        private readonly WebDriverManagerConfig _config = new WebDriverManagerConfig
        {
            Binary = "chromedriver.exe",
            Url = "https://chromedriver.storage.googleapis.com/<version>/chromedriver_win<architecture>.zip",
            PathVariable = "webdriver.chrome.driver",
            Architecture = Architecture.x32.ToString().Replace("x", "")
        };

        public string GetLatestVersion()
        {
            try
            {
                var webRequest = WebRequest.Create(@"https://chromedriver.storage.googleapis.com/LATEST_RELEASE");
                using (var response = webRequest.GetResponse())
                {
                    using (var content = response.GetResponseStream())
                    {
                        if (content != null)
                        {
                            using (var reader = new StreamReader(content))
                            {
                                var version = reader.ReadToEnd().Trim();
                                Log?.Info($"Latest chrome driver version is '{version}'");
                                return version;
                            }
                        }
                        Log?.Error("Can't get content from URL");
                        throw new WebDriverManagerException(
                            "Can't get content from URL", new Exception());
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
        {
            _config.Version = GetLatestVersion();
        }

        public ChromeDriverManager(string version)
        {
            _config.Version = version;
            Log?.Info($"Set chrome driver version to: '{version}'");
        }

        public void Init()
        {
            _config.Destination = Path.Combine(Directory.GetCurrentDirectory(), WebDriverManagerConfig.DefaultDestinationFolder);
            Log?.Debug($"Use default chrome driver destination path: '{_config.Destination}'");
            Base();
        }

        public void Init(string destination)
        {
            _config.Destination = destination;
            Log?.Info($"Set custom chrome driver destination path to: '{destination}'");
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