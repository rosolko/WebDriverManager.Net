using System;
using System.IO;
using System.Net;
using WebDriverManager.Clients;
using WebDriverManager.DriverConfigs;
using WebDriverManager.Helpers;
using WebDriverManager.Services;
using WebDriverManager.Services.Impl;

namespace WebDriverManager
{
    public class DriverManager
    {
        private static readonly object Object = new object();

        private IBinaryService _binaryService;
        private readonly IVariableService _variableService;
        private readonly string _downloadDirectory = Directory.GetCurrentDirectory();

        public DriverManager()
        {
            _binaryService = new BinaryService();
            _variableService = new VariableService();
        }

        public DriverManager(string downloadDirectory):this()
        {
            _downloadDirectory = downloadDirectory;
        }

        public DriverManager(IBinaryService binaryService, IVariableService variableService)
        {
            _binaryService = binaryService;
            _variableService = variableService;
        }

        public DriverManager WithProxy(IWebProxy proxy)
        {
            lock (Object)
            {
                _binaryService = new BinaryService {Proxy = proxy};
            }
            ChromeForTestingClient.Proxy = proxy;
            WebRequest.DefaultWebProxy = proxy;
            return this;
        }

        [Obsolete("binaryName parameter is redundant, use SetUpDriver(url, binaryPath)")]
        public string SetUpDriver(string url, string binaryPath, string binaryName)
        {
            lock (Object)
            {
                return SetUpDriverImpl(url, binaryPath);
            }
        }

        public string SetUpDriver(string url, string binaryPath)
        {
            lock (Object)
            {
                return SetUpDriverImpl(url, binaryPath);
            }
        }

        public string SetUpDriver(IDriverConfig config, string version = VersionResolveStrategy.Latest,
            Architecture architecture = Architecture.Auto)
        {
            lock (Object)
            {
                architecture = architecture.Equals(Architecture.Auto)
                    ? ArchitectureHelper.GetArchitecture()
                    : architecture;
                version = GetVersionToDownload(config, version);
                var url = architecture.Equals(Architecture.X32) ? config.GetUrl32() : config.GetUrl64();
                url = UrlHelper.BuildUrl(url, version);
                var binaryPath = Path.Combine(_downloadDirectory, config.GetName(), version, architecture.ToString(), config.GetBinaryName());
                return SetUpDriverImpl(url, binaryPath);
            }
        }

        private string SetUpDriverImpl(string url, string binaryPath)
        {
            var zipPath = FileHelper.GetZipDestination(url);
            binaryPath = _binaryService.SetupBinary(url, zipPath, binaryPath);
            _variableService.SetupVariable(binaryPath);
            return binaryPath;
        }

        private static string GetVersionToDownload(IDriverConfig config, string version)
        {
            switch (version)
            {
                case VersionResolveStrategy.MatchingBrowser: return config.GetMatchingBrowserVersion();
                case VersionResolveStrategy.Latest: return config.GetLatestVersion();
                default: return version;
            }
        }
    }
}
