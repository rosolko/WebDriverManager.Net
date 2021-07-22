using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class RegistryHelper
    {
        public static string GetInstalledBrowserVersion(string executableFileName, string arguments)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return WindowsRegistryHelper.GetInstalledBrowserVersion(executableFileName);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return Task.Run(() => OSXRegistryHelper.GetInstalledBrowserVersion(executableFileName, arguments)).Result;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Task.Run(() => LinuxRegistryHelper.GetInstalledBrowserVersion(executableFileName, arguments)).Result;
                }

                throw new NotImplementedException($"Runtime platform {Environment.OSVersion.Platform} not supported");
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured trying to locate installed browser version for runtime platform {Environment.OSVersion.Platform}", e);
            }
        }
    }
}
