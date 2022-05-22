using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class RegistryHelper
    {
        public static string GetInstalledBrowserVersionLinux(string executableFileName, string arguments)
        {
            try
            {
                var outputVersion = Task.Run(() => VersionHelper.GetVersionFromProcess(executableFileName, arguments))
                    .Result;
                return outputVersion;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"An error occured trying to locate installed browser version for runtime platform {Environment.OSVersion.Platform}",
                    e);
            }
        }

        public static string GetInstalledBrowserVersionLinux(params string[] executableAndArgumentsPairs)
        {
            var length = executableAndArgumentsPairs.Length;
            if (length % 2 == 1) throw new Exception("Please provide arguments for every executable!");

            for (var i = 0; i < length; i += 2)
            {
                var executableFileName = executableAndArgumentsPairs[i];
                var arguments = executableAndArgumentsPairs[i + 1];
                
                var fullPath = GetFullPath(executableFileName);
                if (fullPath != null) return GetInstalledBrowserVersionLinux(fullPath, arguments);
            }

            throw new Exception(
                $"Unable to locate installed browser for runtime platform {Environment.OSVersion.Platform}");
        }

        public static string GetInstalledBrowserVersionOsx(string executableFileName, string arguments)
        {
            try
            {
                var executableFilePath = $"/Applications/{executableFileName}.app/Contents/MacOS/{executableFileName}";
                var outputVersion = Task.Run(() => VersionHelper.GetVersionFromProcess(executableFilePath, arguments))
                    .Result;
                return outputVersion.Replace($"{executableFileName} ", "");
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"An error occured trying to locate installed browser version for runtime platform {Environment.OSVersion.Platform}",
                    e);
            }
        }

        public static string GetInstalledBrowserVersionWin(string executableFileName)
        {
            const string currentUserRegistryPathPattern =
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";
            const string localMachineRegistryPathPattern =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\<executableFileName>";

            var currentUserPath = Microsoft.Win32.Registry.GetValue(
                currentUserRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (currentUserPath != null)
            {
                return FileVersionInfo.GetVersionInfo(currentUserPath.ToString()).FileVersion;
            }

            var localMachinePath = Microsoft.Win32.Registry.GetValue(
                localMachineRegistryPathPattern.Replace("<executableFileName>", executableFileName), "", null);
            if (localMachinePath != null)
            {
                return FileVersionInfo.GetVersionInfo(localMachinePath.ToString()).FileVersion;
            }

            return null;
        }

        /// <summary>
        /// Checks if a provided file name can be found in either the current working directory or the <c>PATH</c>
        /// environment variable.
        /// </summary>
        /// <param name="fileName">The file name of the executable, including extension on Windows.</param>
        /// <returns>The full path of the executable or <see langword="null"/> if it doesn't exist.</returns>
        private static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName)) return Path.GetFullPath(fileName);

            var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>();
            foreach (var path in paths)
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath)) return fullPath;
            }

            return null;
        }
    }
}
