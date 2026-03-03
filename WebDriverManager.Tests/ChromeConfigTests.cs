using System;
using System.IO;
using System.Text.RegularExpressions;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using Xunit;

namespace WebDriverManager.Tests
{
    public class ChromeConfigTests : ChromeConfig, IDisposable
    {
        private bool disposedValue;

        [Fact]
        public void VersionTest()
        {
            var version = GetLatestVersion();
            var regex = new Regex(@"^\d+\.\d+.\d+.\d+$");
            Assert.NotEmpty(version);
            Assert.Matches(regex, version);
        }

        [Fact]
        public void DriverDownloadLatestTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        [Fact]
        public void DriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }


        [Fact]
        public void SpecificVersionDriverDownloadTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), "145.0.7632.117");

            Assert.NotEmpty(WebDriverFinder.FindFile(GetBinaryName()));
        }

        /// <summary>
        /// Cleanup after each test by deleting the downloaded file directory
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Directory.Exists(WebDriverFinder.FindFile(GetBinaryName())))
                    {
                        var childDirectory = new DirectoryInfo(WebDriverFinder.FindFile(GetBinaryName()));
                        //Go to parent as child directory is the x64 directory
                        childDirectory.Parent.Delete(true);
                    }
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
