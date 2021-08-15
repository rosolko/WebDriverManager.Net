using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        public IWebProxy Proxy { get; set; }

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
            else if (zipDestination.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                UnZip(zipDestination, binDestination, binaryName);
            }
            else if (zipDestination.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase))
            {
                UnZipTgz(zipDestination, binDestination);
            }

#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var chmod = Process.Start("chmod", $"+x {binDestination}");
                chmod?.WaitForExit();
            }
#endif

            RemoveZip(zipDestination);
            return binDestination;
        }

        public string DownloadZip(string url, string destination)
        {
            if (File.Exists(destination)) return destination;
            if (Proxy != null)
            {
                using (var webClient = new WebClient() {Proxy = Proxy})
                {
                    webClient.DownloadFile(new Uri(url), destination);
                }
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(new Uri(url), destination);
                }
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

        protected void UnZipTgz(string gzArchiveName, string destination)
        {
            using (var inStream = File.OpenRead(gzArchiveName))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    var destFolder = Path.GetDirectoryName(destination);
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.Default))
                    {
                        tarArchive.ExtractContents(destFolder);
                    }
                }
            }
        }

        protected void RemoveZip(string path)
        {
            File.Delete(path);
        }
    }
}
