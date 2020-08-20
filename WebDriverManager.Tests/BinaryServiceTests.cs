using System;
using System.IO;
using System.Net;
using WebDriverManager.Helpers;
using WebDriverManager.Services.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class BinaryServiceTests : BinaryService
    {
        [Fact]
        public void DownloadZipResultNotEmpty()
        {
            const string url = "https://chromedriver.storage.googleapis.com/2.27/chromedriver_win32.zip";
            var destination = FileHelper.GetZipDestination(url);
            FileHelper.CreateDestinationDirectory(destination);
            var result = DownloadZip(url, destination, WebRequest.DefaultWebProxy);
            Assert.NotEmpty(result);
            Assert.True(File.Exists(result));
        }

        [Fact]
        public void UnZipResultNotEmpty()
        {
            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "unzipable.zip");
            var destination = FileHelper.GetBinDestination("Files", "2.0.0", Architecture.X32, "file.txt");
            FileHelper.CreateDestinationDirectory(destination);
            var result = UnZip(zipPath, destination, "file.txt");
            Assert.NotEmpty(result);
            Assert.True(File.Exists(result));
        }

        [Fact]
        public void UnZipTgzResultNotEmpty()
        {
            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "gzip.tar.gz");
            var destination = FileHelper.GetBinDestination("Files", "2.0.0", Architecture.X32, "gzip.txt");
            FileHelper.CreateDestinationDirectory(destination);
            UnZipTgz(zipPath, destination);
            Assert.True(File.Exists(destination));
        }

        [Fact]
        public void RemoveZipTargetMissing()
        {
            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "removable.zip");
            Assert.True(File.Exists(zipPath));
            RemoveZip(zipPath);
            Assert.False(File.Exists(zipPath));
        }

        [Fact]
        public void DownloadZipFileWithProxy()
        {
            const string url = "https://chromedriver.storage.googleapis.com/2.27/chromedriver_win32.zip";
            WebProxyStub proxy = new WebProxyStub();
            var destination = FileHelper.GetZipDestination(url);
            FileHelper.CreateDestinationDirectory(destination);
            var result = DownloadZip(url, destination, proxy);
            Assert.Equal(new Uri(url), proxy.RequestedUri);
            Assert.NotEmpty(result);
            Assert.True(File.Exists(result));
        }

        public class WebProxyStub : IWebProxy
        {
            public Uri RequestedUri { get; set; }
            public ICredentials Credentials { get; set; }
            public Uri GetProxy(Uri destination) => new Uri("http://localhost");
            public bool IsBypassed(Uri host)
            {
                RequestedUri = host;
                return true;
            }
        }
    }
}
