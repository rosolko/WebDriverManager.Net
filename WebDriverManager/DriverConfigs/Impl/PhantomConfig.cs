using System;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class PhantomConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "Phantom";
        }

        public virtual string GetUrl32()
        {
            return "https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-2.1.1-windows.zip";
        }

        public virtual string GetUrl64()
        {
            return GetUrl32();
        }

        public virtual string GetBinaryName()
        {
            return "phantomjs.exe";
        }

        public virtual string GetLatestVersion()
        {
            return "2.1.1";
        }

        public virtual string GetMatchingBrowserVersion()
        {
            throw new NotImplementedException();
        }
    }
}
