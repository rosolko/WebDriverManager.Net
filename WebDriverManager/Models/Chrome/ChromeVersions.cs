using System.Collections.Generic;

namespace WebDriverManager.Models.Chrome
{
    public class ChromeVersions
    {
        public string Timestamp { get; set; }

        public ChromeReleaseChannels Channels { get; set; }

        public IEnumerable<ChromeVersionInfo> Versions { get; set; }
    }
}
