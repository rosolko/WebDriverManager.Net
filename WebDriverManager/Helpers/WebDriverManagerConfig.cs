namespace WebDriverManager.Helpers
{
    public class WebDriverManagerConfig
    {
        public readonly string DefaultDestinationFolder = "Drivers";

        public string Architecture { get; set; } = null;

        public string Binary { get; set; } = null;

        public string Release { get; set; } = null;

        public string Version { get; set; } = null;

        public string Url { get; set; } = null;

        public string Destication { get; set; } = null;

        public string PathVariable { get; set; } = null;
    }
}