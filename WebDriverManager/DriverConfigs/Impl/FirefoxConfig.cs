using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://github.com/mozilla/geckodriver/releases");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll("[class='Link--primary']")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Trim(' ', '\r', '\n');
                return version;
            }
        }

        public virtual string GetMatchingBrowserVersion()
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RegistryHelper.GetInstalledBrowserVersionOsx("Firefox", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RegistryHelper.GetInstalledBrowserVersionLinux("firefox", "--version");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RegistryHelper.GetInstalledBrowserVersionWin("firefox.exe");
            }

            throw new PlatformNotSupportedException("Your operating system is not supported");
#else
            return RegistryHelper.GetInstalledBrowserVersionWin("firefox.exe");
#endif
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
