using System;

namespace WebDriverManager.Helpers
{
    public static class ArchitectureHelper
    {
        public static Architecture GetArchitecture()
        {
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))
                ? Architecture.X32
                : Architecture.X64;
        }
    }
}