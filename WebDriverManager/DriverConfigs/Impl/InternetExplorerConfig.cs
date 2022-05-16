using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using WebDriverManager.Helpers;

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
            return "https://selenium-release.storage.googleapis.com/3.150/IEDriverServer_Win32_3.150.1.zip";
        }

        public virtual string GetUrl64()
        {
            return "https://selenium-release.storage.googleapis.com/3.150/IEDriverServer_x64_3.150.1.zip";
        }

        public virtual string GetBinaryName()
        {
            return "IEDriverServer.exe";
        }

        public virtual string GetLatestVersion()
        {
            return "3.150.1";
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
