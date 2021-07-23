using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebDriverManager.Helpers
{
    public static class VersionHelper
    {
        /*
         * Returns a version number without the revision part.
         *
         * Example: 85.0.4183.83 -> 85.0.4183
         */
        public static string GetVersionWithoutRevision(string version)
        {
            var parsedVersion = Version.Parse(version);
            return $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Build}";
        }

        /// <summary>
        /// Gets the current browser version on the executing system by running a process
        /// </summary>
        /// <param name="executableFileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
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

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            process.Kill();

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return output;
        }
    }
}
