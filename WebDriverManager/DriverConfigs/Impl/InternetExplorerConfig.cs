using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class InternetExplorerConfig : IDriverConfig
    {
        public virtual string GetName()
        {
            return "InternetExplorer";
        }

        public virtual string GetUrl32()
        {
            return "http://selenium-release.storage.googleapis.com/3.141/IEDriverServer_Win32_3.141.59.zip";
        }

        public virtual string GetUrl64()
        {
            return "http://selenium-release.storage.googleapis.com/3.141/IEDriverServer_x64_3.141.59.zip";
        }

        public virtual string GetBinaryName()
        {
            return "IEDriverServer.exe";
        }

        public virtual string GetLatestVersion()
        {
            return "3.141.59";
        }
    }
}