using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace WebDriverManager.Services.Impl
{
    public class BinaryService : IBinaryService
    {
        public IWebProxy Proxy { get; set; }

        [Obsolete("binaryName parameter is redundant, use SetupBinary(url, zipDestination, binDestination)")]
        public string SetupBinary(string url, string zipDestination, string binDestination, string binaryName)
        {
            return SetupBinary(url, zipDestination, binDestination);
        }

        public string SetupBinary(string url, string zipPath, string binaryPath)
        {
            var zipDir = Path.GetDirectoryName(zipPath);
            var binaryDir = Path.GetDirectoryName(binaryPath);
            var binaryName = Path.GetFileName(binaryPath);

            //
            // If the destination already exists, we don't have to do anything
            //
            if (Directory.Exists(binaryDir)) return binaryPath;

            //
            // Download the driver
            //
            Directory.CreateDirectory(zipDir);
            zipPath = DownloadZip(url, zipPath);

            //
            // Copy or extract binary(s) into a staging directory
            //
            var stagingDir = Path.Combine(zipDir, "staging");
            var stagingPath = Path.Combine(stagingDir, binaryName);

            Directory.CreateDirectory(stagingDir);

            if (zipPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(zipPath, stagingPath);
            }
            else if (zipPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                UnZip(zipPath, stagingPath, binaryName);
            }
            else if (zipPath.EndsWith(".tar.gz", StringComparison.OrdinalIgnoreCase))
            {
                UnZipTgz(zipPath, stagingPath);
            }

#if NETSTANDARD
            var needsChmod =
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (needsChmod)
            {
                var chmod = Process.Start("chmod", $"+x {stagingPath}");
                chmod?.WaitForExit();
            }
#endif

            //
            // Create the destination parent directory if it doesn't exist
            //
            var binaryParentDir = Path.GetDirectoryName(binaryDir);
            Directory.CreateDirectory(binaryParentDir);

            //
            // Atomically rename the staging directory to the destination directory
            //
            // If a parallel thread or process races us and wins, the destination will already exist and this will fail
            // with an IOException.
            //
            Exception renameException = null;
            try
            {
                Directory.Move(stagingDir, binaryDir);
            }
            catch (Exception ex)
            {
                renameException = ex;
            }

            //
            // Regardless of what happens, do a best-effort clean up
            //
            try
            {
                if (Directory.Exists(stagingDir)) Directory.Delete(stagingDir, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            try
            {
                RemoveZip(zipPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            //
            // If the destination still doesn't exist, it means the rename failed in an unexpected way
            //
            if (!Directory.Exists(binaryDir))
            {
                throw new Exception($"Error writing {binaryDir}", renameException);
            }

            return binaryPath;
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
