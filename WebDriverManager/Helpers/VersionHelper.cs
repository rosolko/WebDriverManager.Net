using System;
using System.Collections.Generic;
using System.Text;

namespace WebDriverManager.Helpers
{
    internal class VersionHelper
    {
        /*
         * Returns a version number without the revision part.
         *
         * Example: 85.0.4183.83 -> 85.0.4183
         */
        public static string GetVersionSkippingRevision(string version)
        {
            Version versionParsed = Version.Parse(version);

            return versionParsed.Major
                + "." + versionParsed.Minor
                + "." + versionParsed.Build;
        }
    }
}
