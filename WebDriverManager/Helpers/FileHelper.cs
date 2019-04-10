using System;
using System.IO;

namespace WebDriverManager.Helpers
{
    public static class FileHelper
    {
        public static string GetZipDestination(string url)
        {
            var tempDirectory = Path.GetTempPath();
            var guid = Guid.NewGuid().ToString();
            var zipName = Path.GetFileName(url);
            if (zipName == null) throw new ArgumentNullException($"Can't get zip name from URL: {url}");
            return Path.Combine(tempDirectory, guid, zipName);
        }

        public static string GetBinDestination(string driverName, string version, Architecture architecture,
            string binName)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return Path.Combine(currentDirectory, driverName, version, architecture.ToString(), binName);
        }

        public static void CreateDestinationDirectory(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != null) Directory.CreateDirectory(directory);
        }
    }
}