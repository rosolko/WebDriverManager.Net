using System.IO;
using System.IO.Compression;
using System.Net;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
        {
            if (File.Exists(binDestination)) return binDestination;
            if (!File.Exists(zipDestination))
            {
                zipDestination = DownloadZip(url, zipDestination);
            }
            FileHelper.CreateDestinationDirectory(binDestination);
            binDestination = UnZip(zipDestination, binDestination, binaryName);
            RemoveZip(zipDestination);
            return binDestination;
        }

        protected string DownloadZip(string url, string destination)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, destination);
            }
            return destination;
        }

        protected string UnZip(string path, string destination, string name)
        {
            using (var zip = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.Name == name)
                    {
                        entry.ExtractToFile(destination);
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