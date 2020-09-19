using System;

namespace WebDriverManager.Helpers
{
    public static class VersionHelper
    {
        /*
         * Returns a version number without the revision part.
         *
         * Example: 85.0.4183.83 -> 85.0.4183
         */
        public static string GetVersionWithoutRevision(string version)
        {
            var parsedVersion = Version.Parse(version);
            return $"{parsedVersion.Major}.{parsedVersion.Minor}.{parsedVersion.Build}";
        }
    }
}
