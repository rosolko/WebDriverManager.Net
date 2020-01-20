using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class EdgeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://msedgedriver.azureedge.net/<version>/";

        public virtual string GetName()
        {
            return "Edge";
        }

        public virtual string GetUrl32()
        {
            return $"{BaseVersionPatternUrl}edgedriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? $"{BaseVersionPatternUrl}edgedriver_mac64.zip"
                : $"{BaseVersionPatternUrl}edgedriver_win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? "msedgedriver.exe"
                : "msedgedriver";
        }

        public virtual string GetLatestVersion()
        {
            var uri = new Uri("https://msedgedriver.azureedge.net/LATEST_BETA");
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
