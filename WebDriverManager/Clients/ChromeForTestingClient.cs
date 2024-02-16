using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebDriverManager.Models.Chrome;

namespace WebDriverManager.Clients
{
    public static class ChromeForTestingClient
    {
        private static readonly string BaseUrl = "https://googlechromelabs.github.io/chrome-for-testing/";
        
        private static HttpClient _httpClient;

        private static HttpClient HttpClient
        {
            get
            {
                var handler = new HttpClientHandler
                {
                    UseProxy = Proxy != null,
                    Proxy = Proxy
                };

                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(BaseUrl)
                };

                return _httpClient;
            }
        }

        public static IWebProxy Proxy { get; set; }

        public static ChromeVersions GetKnownGoodVersionsWithDownloads()
        {
            return GetResultFromHttpTask<ChromeVersions>(
                HttpClient.GetAsync("known-good-versions-with-downloads.json")
            );
        }

        public static ChromeVersions GetLastKnownGoodVersions()
        {
            return GetResultFromHttpTask<ChromeVersions>(
                HttpClient.GetAsync("last-known-good-versions-with-downloads.json")
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

            return JsonConvert.DeserializeObject<TResult>(readStringTask.Result);
        }
    }
}
