using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class EdgeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://msedgedriver.azureedge.net/<version>/";
        private const string LatestReleaseVersionUrl = "https://msedgedriver.azureedge.net/LATEST_STABLE";

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
                ? "msedgedriver"
                : "msedgedriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            return GetLatestVersion(LatestReleaseVersionUrl);
        }

        public virtual string GetLatestVersion(string url)
        {
            var uri = new Uri(url);
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

        public string GetMatchingBrowserVersion()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RegistryHelper.GetInstalledBrowserVersionOsx("Microsoft Edge", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RegistryHelper.GetInstalledBrowserVersionWin("msedge.exe");
            }

            throw new PlatformNotSupportedException("Your operating system is not supported");
        }
    }
}
