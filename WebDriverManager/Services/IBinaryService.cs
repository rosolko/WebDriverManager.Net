using System;

namespace WebDriverManager.Services
{
    public interface IBinaryService
    {
        [Obsolete("binaryName parameter is redundant, use SetupBinary(url, zipDestination, binDestination)")]
        string SetupBinary(string url, string zipDestination, string binDestination, string binaryName);

        string SetupBinary(string url, string zipDestination, string binDestination);
    }
}
