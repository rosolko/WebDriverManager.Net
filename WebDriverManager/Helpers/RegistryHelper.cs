using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace WebDriverManager.Helpers
{
    internal class RegistryHelper
    {
        private const string CurrentUserRegistryPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";
        private const string LocalMachineRegistryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";

        public static string GetInstalledBrowserVersion(string executableFileName)
        {
            object currentUserPath = Registry.GetValue(CurrentUserRegistryPath.Replace("<executableFileName>", executableFileName), "", null);
            object localMachinePath = Registry.GetValue(LocalMachineRegistryPath.Replace("<executableFileName>", executableFileName), "", null);

            if (currentUserPath != null)
            {
                return FileVersionInfo.GetVersionInfo(currentUserPath.ToString()).FileVersion;
            }

            else if (localMachinePath != null)
            {
                return FileVersionInfo.GetVersionInfo(localMachinePath.ToString()).FileVersion;
            }

            return null;
        }
    }
}
