using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class OSXRegistryHelper
    {
        public static async Task<string> GetInstalledBrowserVersion(string executableFileName, string arguments)
        {
            var browserProcess = Process.Start(
                new ProcessStartInfo
                {
                    FileName = $"/Applications/{executableFileName}.app/Contents/MacOS/{executableFileName}",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            );

            string output = await browserProcess.StandardOutput.ReadToEndAsync();
            string error = await browserProcess.StandardError.ReadToEndAsync();
            browserProcess.WaitForExit();
            browserProcess.Kill();

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            output = output.Replace($"{executableFileName} ", "");
            return output;
        }
    }
}
