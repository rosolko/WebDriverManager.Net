using System;
using System.IO;
using System.Net;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class ChromeConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Chrome";
        }

        public virtual string GetUrl32()
        {
            return "https://chromedriver.storage.googleapis.com/<version>/chromedriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            return GetUrl32();
        }

        public virtual string GetBinaryName()
        {
            return "chromedriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            var webRequest = WebRequest.Create("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");
            using (var response = webRequest.GetResponse())
            {
                using (var content = response.GetResponseStream())
                {
                    if (content == null) throw new Exception("Unable to get latest chrome version");
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