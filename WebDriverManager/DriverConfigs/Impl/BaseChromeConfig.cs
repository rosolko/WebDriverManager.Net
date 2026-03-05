using System;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;
using Architecture = WebDriverManager.Helpers.Architecture;

namespace WebDriverManager.DriverConfigs.Impl
{
    public abstract class BaseChromeConfig : IDriverConfig
    {
        public abstract string GetLatestVersion();
        public abstract string GetMatchingBrowserVersion();

        protected abstract string GetUrl(Architecture architecture);

        public virtual string GetName()
        {
            return "Chrome";
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
            var isWindows = true;
#if NETSTANDARD
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
            var suffix = isWindows ? ".exe" : string.Empty;
            return $"chromedriver{suffix}";
        }

        protected string GetRawBrowserVersion()
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
                    "chromium-browser", "--version",
                    "chrome", "--product-version");
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
