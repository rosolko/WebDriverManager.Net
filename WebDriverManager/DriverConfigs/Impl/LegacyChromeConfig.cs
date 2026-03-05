using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;
using Architecture = WebDriverManager.Helpers.Architecture;

namespace WebDriverManager.DriverConfigs.Impl
{
    /// <summary>
    ///
    /// Works with versions [106.0.5249.61; 114.0.5735.90]
    /// </summary>
    public class LegacyChromeConfig : BaseChromeConfig
    {
        private const string BaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/<version>/";
        private const string LatestReleaseVersionUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE";
        private const string ExactReleaseVersionPatternUrl =
            "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_<version>";

        protected override string GetUrl(Architecture architecture)
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var architectureExtension =
 RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64
                    ? "_arm64"
                    : "64";

                return $"{BaseVersionPatternUrl}chromedriver_mac{architectureExtension}.zip";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"{BaseVersionPatternUrl}chromedriver_linux64.zip";
            }
#endif
            var driverName = architecture == Architecture.X64 ? "chromedriver_win64.zip" : "chromedriver_win32.zip";
            return $"{BaseVersionPatternUrl}{driverName}";
        }

        public override string GetLatestVersion()
        {
            return GetLatestVersionFromUrl(LatestReleaseVersionUrl);
        }

        public override string GetMatchingBrowserVersion()
        {
            var rawChromeBrowserVersion = GetRawBrowserVersion();
            if (string.IsNullOrEmpty(rawChromeBrowserVersion))
            {
                throw new Exception("Not able to get chrome version or not installed");
            }

            var chromeBrowserVersion = VersionHelper.GetVersionWithoutRevision(rawChromeBrowserVersion);
            var url = ExactReleaseVersionPatternUrl.Replace("<version>", chromeBrowserVersion);
            return GetLatestVersionFromUrl(url);
        }

        private static string GetLatestVersionFromUrl(string url)
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
    }
}
