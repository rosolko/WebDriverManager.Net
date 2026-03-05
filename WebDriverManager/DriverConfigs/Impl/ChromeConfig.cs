using System;
using System.Linq;
using System.Runtime.InteropServices;
using WebDriverManager.Clients;
using WebDriverManager.Helpers;
using Architecture = WebDriverManager.Helpers.Architecture;

namespace WebDriverManager.DriverConfigs.Impl
{
    /// <summary>
    /// Works with versions [115.0.5763.0; latest]
    /// </summary>
    public class ChromeConfig : BaseChromeConfig
    {
        private const string BaseVersionPatternUrl =
            "https://storage.googleapis.com/chrome-for-testing-public/<version>/<platform>/chromedriver-<platform>.zip";

        protected override string GetUrl(Architecture architecture)
        {
            string platform = architecture == Architecture.X64 ? "win64" : "win32";
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64
                    ? "mac-arm64"
                    : "mac-x64";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux64";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.X64
                    ? "win64"
                    : "win32";
            }
#endif
            return BaseVersionPatternUrl.Replace("<platform>", platform);
        }

        public override string GetLatestVersion()
        {
            var chromeReleases = ChromeForTestingClient.GetLastKnownGoodVersions();
            return chromeReleases.Channels.Stable.Version;
        }

        public override string GetMatchingBrowserVersion()
        {
            var rawChromeBrowserVersion = GetRawBrowserVersion();
            if (string.IsNullOrEmpty(rawChromeBrowserVersion))
            {
                throw new Exception("Not able to get chrome version or not installed");
            }

            var chromeVersion = VersionHelper.GetVersionWithoutRevision(rawChromeBrowserVersion);
            var knownGoodVersions = ChromeForTestingClient.GetKnownGoodVersionsWithDownloads();
            var chromeVersionInfo =
                knownGoodVersions.Versions.LastOrDefault(info => info.Version.Contains(chromeVersion));
            return chromeVersionInfo?.Version;
        }
    }
}
