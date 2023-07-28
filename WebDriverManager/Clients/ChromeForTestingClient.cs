using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebDriverManager.Models.Chrome;

namespace WebDriverManager.Clients
{
    public static class ChromeForTestingClient
    {
        private static readonly string BaseUrl = "https://googlechromelabs.github.io/chrome-for-testing/";
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly HttpClient _httpClient;

        static ChromeForTestingClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public static ChromeVersions GetKnownGoodVersionsWithDownloads()
        {
            return GetResultFromHttpTask<ChromeVersions>(
                _httpClient.GetAsync("known-good-versions-with-downloads.json")
            );
        }

        public static ChromeVersions GetLastKnownGoodVersions()
        {
            return GetResultFromHttpTask<ChromeVersions>(
                _httpClient.GetAsync("last-known-good-versions-with-downloads.json")
            );
        }

        /// <summary>
        /// Get a HTTP result without causing any deadlocks
        /// <para>See: https://learn.microsoft.com/en-us/archive/blogs/jpsanders/asp-net-do-not-use-task-result-in-main-context</para>
        /// </summary>
        /// <typeparam name="TResult">The type of result to convert the HTTP response to</typeparam>
        /// <param name="taskToRun">The <see cref="HttpResponseMessage"/> task to run</param>
        private static TResult GetResultFromHttpTask<TResult>(Task<HttpResponseMessage> taskToRun)
            where TResult : class
        {
            var httpTask = Task.Run(() => taskToRun);
            httpTask.Wait();

            var readStringTask = Task.Run(() => httpTask.Result.Content.ReadAsStringAsync());
            readStringTask.Wait();

            return JsonSerializer.Deserialize<TResult>(readStringTask.Result, JsonSerializerOptions);
        }
    }
}
