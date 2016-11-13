using System.IO;

namespace WebDriverManager.Helpers
{
    public static class FileHelper
    {
        public static string GetZipDestination(string url)
        {
            var tempDirectory = Path.GetTempPath();
            var zipName = Path.GetFileName(url);
            return Path.Combine(tempDirectory, zipName);
        }

        public static string GetBinDestination(string driverName, string version, Architecture architecture, string binName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(currentDirectory, driverName, version, architecture.ToString(), binName);
        }

        public static void CreateDestinationDirectory(string path)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
        }
    }
}