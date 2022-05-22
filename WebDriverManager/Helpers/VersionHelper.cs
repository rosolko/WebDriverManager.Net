using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class VersionHelper
    {
        /// <summary>
        /// Returns a version number without the revision part.
        /// Example: 85.0.4183.83 -> 85.0.4183
        /// </summary>
        /// <param name="version">Version to process</param>
        /// <returns>Processed version</returns>
        public static string GetVersionWithoutRevision(string version)
        {
            var parsedVersion = Version.Parse(version);
            return $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Build}";
        }

        /// <summary>
        /// Gets the current browser version on the executing system by running a process
        /// </summary>
        /// <param name="executableFileName">Browser executable file name</param>
        /// <param name="arguments">Execution command line arguments</param>
        /// <returns>Browser version received from browser started instance</returns>
        public static async Task<string> GetVersionFromProcess(string executableFileName, string arguments)
        {
            var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = executableFileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            );
            if (process == null) throw new Exception("The process did not start");

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            process.Kill();

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            // Tries to pick out just the version string, e.g. from "Chromium 101.0.4951.64 Arch Linux" pick
            // "101.0.4951.64" and from "Mozilla Firefox 100.0" pick "100.0".
            return output.Split().LastOrDefault(word => Version.TryParse(word, out _)) ?? output;
        }
    }
}
