using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        static readonly object _object = new object();

        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
        {
            if (File.Exists(binDestination)) return binDestination;
            FileHelper.CreateDestinationDirectory(zipDestination);
            zipDestination = DownloadZip(url, zipDestination);
            FileHelper.CreateDestinationDirectory(binDestination);
            if (!zipDestination.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                binDestination = UnZip(zipDestination, binDestination, binaryName);
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
            lock (_object)
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
        }

        protected void RemoveZip(string path)
        {
            File.Delete(path);
        }
    }
}