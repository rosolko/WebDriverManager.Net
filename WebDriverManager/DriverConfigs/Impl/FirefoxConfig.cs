using System;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;
using Architecture = WebDriverManager.Helpers.Architecture;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class FirefoxConfig : IDriverConfig
    {
        private const string DownloadUrl = "https://github.com/mozilla/geckodriver/releases/download/v<version>/geckodriver-v<version>-";

        public virtual string GetName()
        {
            return "Firefox";
        }

        public virtual string GetUrl32()
        {
            return GetUrl(Architecture.X32);
        }

        public virtual string GetUrl64()
        {
            return GetUrl(Architecture.X64);
        }

        public virtual string GetBinaryName()
        {
#if NETSTANDARD
            return "geckodriver" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
#else
            return "geckodriver.exe";
#endif
        }

        public virtual string GetLatestVersion()
        {
            return GitHubHelper.GetLatestReleaseName("mozilla", "geckodriver");
        }

        public virtual string GetMatchingBrowserVersion()
        {
            throw new NotImplementedException();
        }

        private static string GetUrl(Architecture architecture)
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"{DownloadUrl}macos.tar.gz";
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? $"{DownloadUrl}linux{((int)architecture)}.tar.gz"
                : $"{DownloadUrl}win{((int)architecture)}.zip";
#else
            return $"{DownloadUrl}win{(int)architecture}.zip";
#endif
        }
    }
}
