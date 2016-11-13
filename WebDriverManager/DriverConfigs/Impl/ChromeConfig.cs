using System;
using System.IO;
using System.Net;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class ChromeConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Chrome";
        }

        public string GetUrl32()
        {
            return "https://chromedriver.storage.googleapis.com/<version>/chromedriver_win32.zip";
        }

        public string GetUrl64()
        {
            return GetUrl32();
        }

        public string GetBinaryName()
        {
            return "chromedriver.exe";
        }

        public string GetLatestVersion()
        {
            var webRequest = WebRequest.Create(@"https://chromedriver.storage.googleapis.com/LATEST_RELEASE");
            using (var response = webRequest.GetResponse())
            {
                using (var content = response.GetResponseStream())
                {
                    if (content == null)
                        throw new Exception(
                            "Can't get content from URL", new Exception());
                    using (var reader = new StreamReader(content))
                    {
                        var version = reader.ReadToEnd().Trim();
                        return version;
                    }
                }
            }
        }
    }
}