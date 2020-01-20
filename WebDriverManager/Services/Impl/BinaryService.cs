using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
        {
            if (File.Exists(binDestination)) return binDestination;
            FileHelper.CreateDestinationDirectory(zipDestination);
            zipDestination = DownloadZip(url, zipDestination);
            FileHelper.CreateDestinationDirectory(binDestination);

            if (zipDestination.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(zipDestination, binDestination);
            }
            else
            {
                UnZip(zipDestination, binDestination, binaryName);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var chmod = Process.Start("chmod", $"+x {binDestination}");
                chmod.WaitForExit();
            }

            RemoveZip(zipDestination);
            return binDestination;
        }

        protected string DownloadZip(string url, string destination)
        {
            if (File.Exists(destination)) return destination;
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), destination);
            }

            return destination;
        }

        protected string UnZip(string path, string destination, string name)
        {
            var zipName = Path.GetFileName(path);
            if (zipName != null && zipName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
            {
                File.Copy(path, destination);
                return destination;
            }

            using (var zip = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.Name == name)
                    {
                        entry.ExtractToFile(destination, true);
                    }
                }
            }

            return destination;
        }

        protected void RemoveZip(string path)
        {
            File.Delete(path);
        }
    }
}
