using System.IO.Compression;
using System.IO;

namespace WebDriverManager.Helpers
{
    internal static class ArchiveHelper
    {
        public static byte[] UnpackGzip(byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            using (var decompressedStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    gZipStream.CopyTo(decompressedStream);
                }

                return decompressedStream.ToArray();
            }
        }
    }
}
