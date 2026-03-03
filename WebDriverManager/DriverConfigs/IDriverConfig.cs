namespace WebDriverManager.DriverConfigs
{
    public interface IDriverConfig
    {
        string GetName();
        string GetUrl32();
        string GetUrl64();
        string GetBinaryName();
        string GetLatestVersion();
        string GetMatchingBrowserVersion();
        string GetMatchingExplicitRequest(string version);
    }
}
