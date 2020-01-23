using System;

namespace WebDriverManager.Helpers
{
    public static class ArchitectureHelper
    {
        public static Architecture GetArchitecture()
        {
            return Environment.Is64BitOperatingSystem
                ? Architecture.X64
                : Architecture.X32;
        }
    }
}
