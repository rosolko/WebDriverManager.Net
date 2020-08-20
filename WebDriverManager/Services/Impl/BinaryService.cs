using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Tar;
using WebDriverManager.Helpers;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName, IWebProxy proxy)
        {
            if (File.Exists(binDestination)) return binDestination;
            FileHelper.CreateDestinationDirectory(zipDestination);
            zipDestination = DownloadZip(url, zipDestination, proxy);
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

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var chmod = Process.Start("chmod", $"+x {binDestination}");
                chmod.WaitForExit();
            }

            RemoveZip(zipDestination);
            return binDestination;
        }

        protected string DownloadZip(string url, string destination, IWebProxy proxy)
        {
            if (File.Exists(destination)) return destination;
            using (var webClient = new WebClient() { Proxy = proxy })
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

        protected void UnZipTgz(string gzArchiveName, string destination)
        {
            using (var inStream = File.OpenRead(gzArchiveName))
            {
                using (var gzipStream = new GZipInputStream(inStream))
                {
                    var destFolder = Path.GetDirectoryName(destination);
                    using (var tarArchive = TarArchive.CreateInputTarArchive(gzipStream))
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
