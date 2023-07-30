namespace WebDriverManager.Models.Chrome
{
    public class ChromeReleaseTrack
    {
        public string Channel { get; set; }

        public string Version { get; set; }

        public string Revision { get; set; }

        public ChromeDownload Downloads { get; set; }
    }
}
