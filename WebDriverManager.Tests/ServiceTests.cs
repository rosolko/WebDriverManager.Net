using System;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Services.Impl;
using Xunit;

namespace WebDriverManager.Tests
{
    public class ServiceTests
    {
        private readonly BinaryService _customBinaryService;
        private readonly VariableService _customVariableService;
        private readonly ChromeConfig _chromeConfig;

        public ServiceTests()
        {
            _customBinaryService = new BinaryService();
            _customVariableService = new VariableService();
            _chromeConfig = new ChromeConfig();
        }

        [Fact]
        public void CustomServiceTest()
        {
            new DriverManager(_customBinaryService, _customVariableService).SetUpDriver(_chromeConfig);
            var pathVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            Assert.NotNull(pathVariable);
            Assert.Contains(_chromeConfig.GetName(), pathVariable);
        }
    }
}
