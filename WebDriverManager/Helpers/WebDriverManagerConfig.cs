namespace WebDriverManager.Helpers
{
    public class WebDriverManagerConfig
    {
        public readonly string DefaultDestinationFolder = "Drivers";

        public string architecture { get; set; } = null;

        public string binary { get; set; } = null;

        public string release { get; set; } = null;

        public string version { get; set; } = null;

        public string url { get; set; } = null;

        public string destication { get; set; } = null;

        public string pathVariable { get; set; } = null;
    }
}
