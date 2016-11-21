using System.IO;
using WebDriverManager.Helpers;
using WebDriverManager.Services.Impl;
using Xunit;

namespace IntegrationTests
{
    public class BinaryServiceTests : BinaryService
    {
        [Fact]
        public void DownloadZipResultNotEmpty()
        {
            var url = "https://chromedriver.storage.googleapis.com/2.25/chromedriver_win32.zip";
            var destination = FileHelper.GetZipDestination(url);
            var result = DownloadZip(url, destination);
            Assert.NotEmpty(result);
            Assert.True(File.Exists(result));
        }

        [Fact]
        public void UnZipResultNotEmpty()
        {
            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\unzipable.zip");
            var destination = FileHelper.GetBinDestination("Files", "2.0.0", Architecture.X32, "file.txt");
            FileHelper.CreateDestinationDirectory(destination);
            var result = UnZip(zipPath, destination, "file.txt");
            Assert.NotEmpty(result);
            Assert.True(File.Exists(result));
        }

        [Fact]
        public void RemoveZipTargetMissing()
        {
            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\removable.zip");
            RemoveZip(zipPath);
            Assert.False(File.Exists(zipPath));
        }
    }
}