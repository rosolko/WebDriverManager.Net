using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebDriverManager.DriverConfigs.Impl
{
    public class AppiumConfig : IDriverConfig
    {
        public string GetName()
        {
            return "Appium";
        }

        public string GetUrl32()
        {
            return "https://bitbucket.org/appium/appium.app/downloads/AppiumForWindows_<version>.zip";
        }

        public string GetUrl64()
        {
            return GetUrl32();
        }

        public string GetBinaryName()
        {
            return "appium-installer.exe";
        }

        public string GetLatestVersion()
        {
            using (var client = new WebClient())
            {
                var doc = new HtmlDocument();
                var htmlCode = client.DownloadString("https://bitbucket.org/appium/appium.app/downloads");
                doc.LoadHtml(htmlCode);
                var itemList =
                    doc.DocumentNode.SelectNodes(
                            "//tr[@class='iterable-item']/td[@class='name']/a[contains(.,'AppiumForWindows_')]")
                        .Select(p => p.InnerText)
                        .ToList();
                var item = itemList.FirstOrDefault();
                var version = item?.Substring(item.IndexOf(item.Split('_')[1], StringComparison.Ordinal))
                    .Split('.')[0];
                return version;
            }
        }
    }
}