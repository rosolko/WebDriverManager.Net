using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebDriverManager.Models.Chrome;

namespace WebDriverManager.Helpers
{
    public class ChromeForTestingClient
    {
        private static readonly string BaseUrl = "https://googlechromelabs.github.io/chrome-for-testing/";
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        public ChromeForTestingClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<ChromeVersions> GetKnownGoodVersionsWithDownloads()
        {
            var response = await _httpClient.GetAsync("known-good-versions-with-downloads.json");
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChromeVersions>(jsonString, JsonSerializerOptions);
        }

        public async Task<ChromeVersions> GetLastKnownGoodVersions()
        {
            var response = await _httpClient.GetAsync("last-known-good-versions-with-downloads.json");
            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChromeVersions>(jsonString, JsonSerializerOptions);
        }
    }
}
