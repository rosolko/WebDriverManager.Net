using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class RegistryHelper
    {
        public static string GetInstalledBrowserVersionLinux(string executableFileName, string arguments)
        {
            try
            {
                var outputVersion = Task.Run(() => VersionHelper.GetVersionFromProcess(executableFileName, arguments)).Result;
                return outputVersion;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured trying to locate installed browser version for runtime platform {Environment.OSVersion.Platform}", e);
            }
        }

        public static string GetInstalledBrowserVersionOSX(string executableFileName, string arguments)
        {
            try
            {
                var executableFilePath = $"/Applications/{executableFileName}.app/Contents/MacOS/{executableFileName}";
                var outputVersion = Task.Run(() => VersionHelper.GetVersionFromProcess(executableFilePath, arguments)).Result;
                return outputVersion.Replace($"{executableFileName} ", "");
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured trying to locate installed browser version for runtime platform {Environment.OSVersion.Platform}", e);
            }
        }

        public static string GetInstalledBrowserVersionWin(string executableFileName)
        {
            var currentUserRegistryPathPattern = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";
            var localMachineRegistryPathPattern = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";

            var currentUserPath = Microsoft.Win32.Registry.GetValue(currentUserRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (currentUserPath != null)
            {
                return FileVersionInfo.GetVersionInfo(currentUserPath.ToString()).FileVersion;
            }

            var localMachinePath = Microsoft.Win32.Registry.GetValue(localMachineRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (localMachinePath != null)
            {
                return FileVersionInfo.GetVersionInfo(localMachinePath.ToString()).FileVersion;
            }

            return null;
        }
    }
}
