using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class ChromeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/<version>/";

        public virtual string GetName()
        {
            return "Chrome";
        }

        public virtual string GetUrl32()
        {
            return GetUrl();
        }

        public virtual string GetUrl64()
        {
            return GetUrl();
        }

        private string GetUrl()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"{BaseVersionPatternUrl}chromedriver_mac64.zip";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"{BaseVersionPatternUrl}chromedriver_linux64.zip";
            }

            return $"{BaseVersionPatternUrl}chromedriver_win32.zip";
        }

        public virtual string GetBinaryName()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var suffix = isWindows ? ".exe" : string.Empty;
            return $"chromedriver{suffix}";
        }

        public virtual string GetLatestVersion()
        {
            return GetVersion("LATEST_RELEASE");
        }

        public string GetVersion(string versionString)
        {
            var uri = new Uri(new Uri("https://chromedriver.storage.googleapis.com/"), versionString);
            var webRequest = WebRequest.Create(uri);
            using (var response = webRequest.GetResponse())
            {
                using (var content = response.GetResponseStream())
                {
                    if (content == null) throw new ArgumentNullException($"Can't get content from URL: {uri}");
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
