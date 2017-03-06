using System;
using System.IO;
using WebDriverManager.Services.Impl;
using Xunit;

namespace IntegrationTests
{
    public class VariableServiceTests : VariableService
    {
        [Fact, Trait("Category", "Variable")]
        public void UpdatePathResultValid()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "file.txt");
            UpdatePath(filePath);
            var pathVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            var variable = Path.GetDirectoryName(filePath);
            Assert.NotNull(pathVariable);
            Assert.Contains(variable, pathVariable);
        }
    }
}