using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebDriverManager.Helpers
{
    internal static class GitHubHelper
    {
        static GitHubHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static string GetLatestReleaseName(string owner, string repo)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(owner) is false);
            Debug.Assert(string.IsNullOrWhiteSpace(repo) is false);

            string json;

            using (var client = new WebClient())
            {
                // Use the specific version of the API for stability.
                // <https://docs.github.com/en/rest/overview/media-types>
                client.Headers.Add(HttpRequestHeader.Accept, "application/vnd.github.v3+json");

                // All API requests MUST include a valid User-Agent header. Requests with no User-Agent header will be rejected.
                // <https://docs.github.com/en/rest/overview/resources-in-the-rest-api#user-agent-required>
                // TODO: Detect current package version.
                client.Headers.Add(HttpRequestHeader.UserAgent, "2.12.2");

                json = client.DownloadString($"https://api.github.com/repos/{owner}/{repo}/releases/latest");
            }

            var releaseObject = JsonConvert.DeserializeObject<JObject>(json);

            return releaseObject.Value<string>("name");
        }
    }
}
