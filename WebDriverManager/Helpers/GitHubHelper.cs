using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebDriverManager.Helpers
{
    internal static class GitHubHelper
    {
        private static readonly string _assemblyUserAgent;

        static GitHubHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var version = typeof(GitHubHelper).Assembly.GetName().Version;
            _assemblyUserAgent = $"WebDriverManager.Net/{version}";
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
                client.Headers.Add(HttpRequestHeader.UserAgent, _assemblyUserAgent);

                json = client.DownloadString($"https://api.github.com/repos/{owner}/{repo}/releases/latest");
            }

            var releaseObject = JsonConvert.DeserializeObject<JObject>(json);

            return releaseObject.Value<string>("name");
        }
    }
}
