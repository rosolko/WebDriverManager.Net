using System;
using WebDriverManager.Helpers;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class OperaConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Opera";
        }

        public virtual string GetUrl32()
        {
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win32.zip";
        }

        public virtual string GetUrl64()
        {
            return
                "https://github.com/operasoftware/operachromiumdriver/releases/download/v.<version>/operadriver_win64.zip";
        }

        public virtual string GetBinaryName()
        {
            return "operadriver.exe";
        }

        public virtual string GetLatestVersion()
        {
            return GitHubHelper.GetLatestReleaseName("operasoftware", "operachromiumdriver");
        }

        public virtual string GetMatchingBrowserVersion()
        {
            throw new NotImplementedException();
        }
    }
}
