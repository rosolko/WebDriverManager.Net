using System.Diagnostics;

namespace WebDriverManager.Helpers
{
    public static class WindowsRegistryHelper
    {
        private const string CurrentUserRegistryPathPattern = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";
        private const string LocalMachineRegistryPathPattern = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";

        public static string GetInstalledBrowserVersion(string executableFileName)
        {
            var currentUserPath = Microsoft.Win32.Registry.GetValue(CurrentUserRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (currentUserPath != null)
            {
                return FileVersionInfo.GetVersionInfo(currentUserPath.ToString()).FileVersion;
            }

            var localMachinePath = Microsoft.Win32.Registry.GetValue(LocalMachineRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (localMachinePath != null)
            {
                return FileVersionInfo.GetVersionInfo(localMachinePath.ToString()).FileVersion;
            }

            return null;
        }
    }
}
