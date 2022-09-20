using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
            return "4.3.0";
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
