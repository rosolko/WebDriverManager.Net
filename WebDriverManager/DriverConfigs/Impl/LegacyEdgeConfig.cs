using System;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class LegacyEdgeConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Edge";
        }

        public virtual string GetUrl32()
        {
            return
                "https://download.microsoft.com/download/F/8/A/F8AF50AB-3C3A-4BC4-8773-DC27B32988DD/MicrosoftWebDriver.exe";
        }

        public virtual string GetUrl64()
        {
            return GetUrl32();
        }

        public virtual string GetBinaryName()
        {
            return "MicrosoftWebDriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            return "6.17134";
        }

        public virtual string GetMatchingBrowserInstalledVersion()
        {
            throw new NotImplementedException();
        }
    }
}
