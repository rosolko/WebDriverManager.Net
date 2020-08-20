using System.Net;
using WebDriverManager.DriverConfigs;
using WebDriverManager.Helpers;
using WebDriverManager.Services;
using WebDriverManager.Services.Impl;

namespace WebDriverManager
{
    public class DriverManager
    {
        static readonly object _object = new object();

        private readonly IBinaryService _binaryService;
        private readonly IVariableService _variableService;

        public DriverManager()
        {
            _binaryService = new BinaryService();
            _variableService = new VariableService();
        }

        public DriverManager(IBinaryService binaryService, IVariableService variableService)
        {
            _binaryService = binaryService;
            _variableService = variableService;
        }

        public IWebProxy Proxy { get; set; }

        public void SetUpDriver(string url, string binaryPath, string binaryName)
        {
            IWebProxy proxy = WebRequest.DefaultWebProxy;
            SetUpDriver(url, binaryPath, binaryName, proxy);
        }
        public void SetUpDriver(string url, string binaryPath, string binaryName, IWebProxy proxy)
        {
            var zipPath = FileHelper.GetZipDestination(url);
            binaryPath = _binaryService.SetupBinary(url, zipPath, binaryPath, binaryName, proxy);
            _variableService.SetupVariable(binaryPath);
        }

        public void SetUpDriver(IDriverConfig config, string version = "Latest",
            Architecture architecture = Architecture.Auto)
        {
            IWebProxy proxy = WebRequest.DefaultWebProxy;
            SetUpDriver(config, proxy, version, architecture);
        }
        public void SetUpDriver(IDriverConfig config, IWebProxy proxy, string version = "Latest",
                Architecture architecture = Architecture.Auto)
        {
            lock (_object)
            {
                architecture = architecture.Equals(Architecture.Auto)
                    ? ArchitectureHelper.GetArchitecture()
                    : architecture;
                version = version.Equals("Latest") ? config.GetLatestVersion() : version;
                var url = architecture.Equals(Architecture.X32) ? config.GetUrl32() : config.GetUrl64();
                url = UrlHelper.BuildUrl(url, version);
                var binaryPath = FileHelper.GetBinDestination(config.GetName(), version, architecture,
                    config.GetBinaryName());
                SetUpDriver(url, binaryPath, config.GetBinaryName(), proxy);
            }
        }
    }
}
