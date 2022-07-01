using System;
using System.IO;
using WebDriverManager.Helpers;
using WebDriverManager.Services.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class BinaryServiceTests : BinaryService
    {
        [Fact]
        public void ProxySetBySystemVariableHttp()
        {
            const string url = "https://chromedriver.storage.googleapis.com/2.27/chromedriver_win32.zip";
            const string httpName = "HTTP_PROXY";
            const string proxyUrl = "http://myproxy:8080/";
            Environment.SetEnvironmentVariable(httpName, proxyUrl);
            CheckProxySystemVariables();
            Assert.NotNull(Proxy);
            Assert.Equal(proxyUrl, Proxy.GetProxy(new Uri(url)).AbsoluteUri);
        }

        [Fact]
        public void ProxySetBySystemVariableHttps()
        {
            const string url = "https://chromedriver.storage.googleapis.com/2.27/chromedriver_win32.zip";
            const string httpName = "HTTPS_PROXY";
            const string proxyUrl = "http://myproxy:8080/";
            Environment.SetEnvironmentVariable(httpName, proxyUrl);
            CheckProxySystemVariables();
            Assert.NotNull(Proxy);
            Assert.Equal(proxyUrl, Proxy.GetProxy(new Uri(url)).AbsoluteUri);
        }


        [Fact]
        public void NoProxyBySystemVariable()
        {
            CheckProxySystemVariables();
            Assert.Null(Proxy);
        }

        [Fact]
        public void DownloadZipResultNotEmpty()
        {
            const string url = "https://chromedriver.storage.googleapis.com/2.27/chromedriver_win32.zip";
            var destination = FileHelper.GetZipDestination(url);
            FileHelper.CreateDestinationDirectory(destination);
            var result = DownloadZip(url, destination);
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
    }
}
