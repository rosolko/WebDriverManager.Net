using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;
using WebDriverManager.Models.Chrome;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class ChromeConfig : IDriverConfig
    {
        private const string BaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/<version>/";
        private const string ExactReleaseVersionPatternUrl = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_<version>";

        /// <summary>
        /// The minimum version required to download chrome drivers from Chrome for Testing API's 
        /// </summary>
        private static readonly Version MinChromeForTestingDriverVersion = new Version("115.0.5763.0");

        /// <summary>
        /// The minimum version of chrome driver required to reference download URLs via the "arm64" extension
        /// </summary>
        private static readonly Version MinArm64ExtensionVersion = new Version("106.0.5249.61");

        private readonly ChromeForTestingClient _chromeForTestingClient;
        private ChromeVersionInfo _chromeVersionInfo;
        private string _chromeVersion;

        public ChromeConfig()
        {
            _chromeForTestingClient = new ChromeForTestingClient();
        }

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
            // Handle newer versions of chrome driver only being available for download via the Chrome for Testing API's
            // whilst retaining backwards compatibility for older versions of chrome/chrome driver.
            if (_chromeVersionInfo != null)
            {
                return GetUrlFromChromeForTestingApi();
            }

#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // Handle older versions of chrome driver arm64 builds that are tagged with 64_m1 instead of arm64.
                // See: https://chromedriver.storage.googleapis.com/index.html?path=106.0.5249.21/
                var useM1Prefix = new Version(_chromeVersion) < MinArm64ExtensionVersion;
                var armArchitectureExtension = useM1Prefix
                    ? "64_m1"
                    : "_arm64";

                var architectureExtension = RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64
                    ? armArchitectureExtension
                    : "64";

                return $"{BaseVersionPatternUrl}chromedriver_mac{architectureExtension}.zip";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"{BaseVersionPatternUrl}chromedriver_linux64.zip";
            }
#endif

            return $"{BaseVersionPatternUrl}chromedriver_win32.zip";
        }

        public virtual string GetBinaryName()
        {
#if NETSTANDARD
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            var isWindows = true;
#endif
            var suffix = isWindows ? ".exe" : string.Empty;
            return $"chromedriver{suffix}";
        }

        public virtual string GetLatestVersion()
        {
            var chromeReleases = _chromeForTestingClient.GetLastKnownGoodVersions().Result;
            var chromeStable = chromeReleases.Channels.Stable;

            _chromeVersionInfo = new ChromeVersionInfo
            {
                Downloads = chromeStable.Downloads
            };

            return chromeStable.Version;
        }

        public virtual string GetMatchingBrowserVersion()
        {
            var rawChromeBrowserVersion = GetRawBrowserVersion();
            if (string.IsNullOrEmpty(rawChromeBrowserVersion))
            {
                throw new Exception("Not able to get chrome version or not installed");
            }

            var chromeVersion = VersionHelper.GetVersionWithoutRevision(rawChromeBrowserVersion);

            // Handle downloading versions of the chrome webdriver less than what's supported by the Chrome for Testing known good versions API
            // See https://googlechromelabs.github.io/chrome-for-testing for more info
            var matchedVersion = new Version(chromeVersion);
            if (matchedVersion < MinChromeForTestingDriverVersion)
            {
                var url = ExactReleaseVersionPatternUrl.Replace("<version>", chromeVersion);
                _chromeVersion = GetVersionFromChromeStorage(url);
            }
            else
            {
                _chromeVersion = GetVersionFromChromeForTestingApi(chromeVersion).Version;
            }

            return _chromeVersion;
        }

        /// <summary>
        /// Retrieves a chrome driver version string from https://chromedriver.storage.googleapis.com
        /// </summary>
        /// <param name="url">The request URL</param>
        /// <returns>A chrome driver version string</returns>
        private static string GetVersionFromChromeStorage(string url)
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

        /// <summary>
        /// Retrieves a chrome driver version string from https://googlechromelabs.github.io/chrome-for-testing
        /// </summary>
        /// <param name="version">The desired version to download</param>
        /// <returns>Chrome driver version info (version number, revision number, download URLs)</returns>
        private ChromeVersionInfo GetVersionFromChromeForTestingApi(string version)
        {
            var knownGoodVersions = _chromeForTestingClient.GetKnownGoodVersionsWithDownloads().Result;
            _chromeVersionInfo = knownGoodVersions.Versions.FirstOrDefault(cV => cV.Version == version);

            return _chromeVersionInfo;
        }

        private string GetUrlFromChromeForTestingApi()
        {
            var platform = "win32";

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
            var result = _chromeVersionInfo.Downloads.ChromeDriver
                .FirstOrDefault(driver => driver.Platform == platform);

            return result.Url;
        }

        private string GetRawBrowserVersion()
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RegistryHelper.GetInstalledBrowserVersionOsx("Google Chrome", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RegistryHelper.GetInstalledBrowserVersionLinux(
                    "google-chrome", "--product-version",
                    "chromium", "--version",
                    "chromium-browser", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RegistryHelper.GetInstalledBrowserVersionWin("chrome.exe");
            }

            throw new PlatformNotSupportedException("Your operating system is not supported");
#else
            return RegistryHelper.GetInstalledBrowserVersionWin("chrome.exe");
#endif
        }
    }
}
