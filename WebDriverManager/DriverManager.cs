using System;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager.Services;
using WebDriverManager.Services.Impl;

namespace WebDriverManager
{
    public class DriverManager
    {
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

        public void SetUpDriver(string url, string binaryPath, string binaryName)
        {
            var zipPath = FileHelper.GetZipDestination(url);
            binaryPath = _binaryService.SetupBinary(url, zipPath, binaryPath, binaryName);
            _variableService.SetupVariable(binaryPath);
        }

        public void SetUpDriver(IDriverConfig config, string version = "Latest", Architecture architecture = Architecture.Auto)
        {
            architecture = architecture.Equals(Architecture.Auto) ? ArchitectureHelper.GetArchitecture() : architecture;
            version = version.Equals("Latest") ? config.GetLatestVersion() : version;

            if (version.StartsWith("LATEST_RELEASE_", StringComparison.OrdinalIgnoreCase) && config is ChromeConfig)
            {
                version = ((ChromeConfig) config).GetVersion(version);
            }

            var url = architecture.Equals(Architecture.X32) ? config.GetUrl32() : config.GetUrl64();
            url = UrlHelper.BuildUrl(url, version);
            var binaryPath = FileHelper.GetBinDestination(config.GetName(), version, architecture,
                config.GetBinaryName());
            SetUpDriver(url, binaryPath, config.GetBinaryName());
        }
    }
}
