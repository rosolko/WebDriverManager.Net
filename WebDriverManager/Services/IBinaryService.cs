using System.Net;

namespace WebDriverManager.Services
{
    public interface IBinaryService
    {
        string SetupBinary(string url, string zipDestination, string binDestination, string binaryName);
        IWebProxy Proxy { get; set; }
    }
}
