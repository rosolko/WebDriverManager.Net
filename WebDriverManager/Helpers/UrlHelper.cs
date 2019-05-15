using System;

namespace WebDriverManager.Helpers
{
    public static class UrlHelper
    {
        public static string BuildUrl(string url, string version)
        {
            return url
                .Replace("<version>", version)
                .Replace("<release>", version.Substring(0, version.LastIndexOf(".", StringComparison.CurrentCulture)));
        }
    }
}
