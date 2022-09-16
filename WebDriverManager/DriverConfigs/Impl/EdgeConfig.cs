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
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var architectureExtension =
                    RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64
                        ? "_m1"
                        : "";
                return $"{BaseVersionPatternUrl}edgedriver_mac64{architectureExtension}.zip";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"{BaseVersionPatternUrl}edgedriver_linux64.zip";
            }

            return $"{BaseVersionPatternUrl}edgedriver_win64.zip";
#else
            return $"{BaseVersionPatternUrl}edgedriver_win64.zip";
#endif
        }

        public virtual string GetBinaryName()
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "msedgedriver";
            }

            return "msedgedriver.exe";
#else
            return "msedgedriver.exe";
#endif
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
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RegistryHelper.GetInstalledBrowserVersionOsx("Microsoft Edge", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RegistryHelper.GetInstalledBrowserVersionLinux("microsoft-edge", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RegistryHelper.GetInstalledBrowserVersionWin("msedge.exe");
            }

            throw new PlatformNotSupportedException("Your operating system is not supported");
#else
            return RegistryHelper.GetInstalledBrowserVersionWin("msedge.exe");
#endif
        }
    }
}
