using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class InternetExplorerConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "InternetExplorer";
        }

        public virtual string GetUrl32()
        {
            return
                "https://github.com/SeleniumHQ/selenium/releases/download/selenium-<version>/IEDriverServer_Win32_<version>.zip";
        }

        public virtual string GetUrl64()
        {
            return
                "https://github.com/SeleniumHQ/selenium/releases/download/selenium-<version>/IEDriverServer_x64_<version>.zip";
        }

        public virtual string GetBinaryName()
        {
            return "IEDriverServer.exe";
        }

        public virtual string GetLatestVersion()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new WebClient())
            {
                var htmlCode = client.DownloadString("https://github.com/SeleniumHQ/selenium/releases/latest");
                var parser = new HtmlParser();
                var document = parser.ParseDocument(htmlCode);
                var version = document.QuerySelectorAll("h1.d-inline")
                    .Select(element => element.TextContent)
                    .FirstOrDefault()
                    ?.Replace("Selenium", "")
                    .Trim(' ', '\r', '\n');
                return version;
            }
        }

        public virtual string GetMatchingBrowserVersion()
        {
#if NETSTANDARD
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Your operating system is not supported");
            }
#endif

            return (string)Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Internet Explorer",
                "svcVersion",
                "Latest");
        }
    }
}
