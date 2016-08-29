namespace WebDriverManager.Helpers
{
    using NLog;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;

    public static class WebDriverManager
    {
        private static Logger hLog = LogManager.GetCurrentClassLogger();

        private static string desticationFolder { get; set; } = null;

        private static string desticationZip { get; set; } = null;

        private static string desticationFile { get; set; } = null;

        private static string zip { get; set; } = null;

        private static bool isNew { get; set; } = false;

        /// <summary>
        /// Build browser driver download URL from mock using config parameters
        /// </summary>
        /// <param name="baseUrl">URL mock</param>
        /// <param name="release">Browser driver release number</param>
        /// <param name="version">Browser driver version number</param>
        /// <param name="architecture">Target browser driver architecture</param>
        /// <returns>Browser driver download URL</returns>
        private static string BuildUrl(string baseUrl, string release, string version, string architecture)
        {
            try
            {
                return baseUrl
                    .Replace("<release>", release)
                    .Replace("<version>", version)
                    .Replace("<architecture>", architecture);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during building browser driver archive download URL");
                throw new WebDriverManagerException("Error occurred during building browser driver archive download URL", ex);
            }
        }

        /// <summary>
        /// Prepare destination folder based on path
        /// </summary>
        /// <param name="path">Default or custom destination folder path</param>
        private static void PrepareCatalogs(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during browser driver catalog preparation");
                throw new WebDriverManagerException("Error occurred during browser driver catalog preparation", ex);
            }
        }

        /// <summary>
        /// Get destination browser driver archive name based on browser driver download URL
        /// </summary>
        /// <param name="hreflink">Browser driver download URL</param>
        /// <returns>Destination browser driver archive file name</returns>
        private static string ZipFileName(string hreflink)
        {
            try
            {
                return Path.GetFileName(hreflink);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during getting browser driver archive name");
                throw new WebDriverManagerException("Error occurred during getting browser driver archive name", ex);
            }
        }

        /// <summary>
        /// Download browser driver archive
        /// </summary>
        /// <param name="config">Specific browser driver config</param>
        public static void Download(WebDriverManagerConfig config)
        {
            using (WebClient webClient = new WebClient())
            {
                string url = BuildUrl(config.url, config.release, config.version, config.architecture);
                zip = ZipFileName(url);
                desticationFolder = Path.Combine(config.destication, Path.GetFileNameWithoutExtension(config.binary), config.version, config.architecture);
                desticationZip = Path.Combine(desticationFolder, zip);
                desticationFile = Path.Combine(desticationFolder, config.binary);
                try
                {
                    if (!File.Exists(desticationFile))
                    {
                        PrepareCatalogs(desticationFolder);
                        webClient.DownloadFile(url, desticationZip);
                    }
                }
                catch (Exception ex)
                {
                    hLog.Error(ex, "Error occurred during browser driver archive downloading");
                    throw new WebDriverManagerException("Error occurred during browser driver archive downloading", ex);
                }
            }
        }

        /// <summary>
        /// Extract browser driver archive
        /// </summary>
        /// <param name="config">Specific browser driver config </param>
        public static void Unzip(WebDriverManagerConfig config)
        {
            try
            {
                desticationFile = Path.Combine(desticationFolder, config.binary);

                if (!File.Exists(desticationFile))
                {
                    using (ZipArchive zip = ZipFile.Open(desticationZip, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            if (entry.Name == config.binary)
                            {
                                entry.ExtractToFile(desticationFile);
                            }
                        }
                    }
                    isNew = true;
                }
                else
                    isNew = false;
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during browser driver archive extracting");
                throw new WebDriverManagerException("Error occurred during browser driver archive extracting", ex);
            }
        }

        /// <summary>
        /// Delete extracted browser driver archive
        /// </summary>
        public static void Clean()
        {
            try
            {
                if (File.Exists(desticationZip))
                    File.Delete(desticationZip);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during deleting extracted browser driver archive");
                throw new WebDriverManagerException("Error occurred during deleting extracted browser driver archive", ex);
            }
        }

        /// <summary>
        /// Add browser driver environment variable
        /// </summary>
        /// <param name="variable">Environment variable</param>
        public static void AddEnvironmentVariable(string variable)
        {
            try
            {
                var variableValue = Environment.GetEnvironmentVariable(variable);
                if (variableValue == null || !variableValue.Equals(desticationFolder))
                    Environment.SetEnvironmentVariable(variable, desticationFolder, EnvironmentVariableTarget.Machine);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during adding(updating) browser driver environment variable");
                throw new WebDriverManagerException("Error occurred during adding(updating) browser driver environment variable", ex);
            }
        }

        /// <summary>
        /// Update browser driver environment variable if it's already exist and different from current
        /// </summary>
        /// <param name="variable">Environment variable</param>
        /// NOTE: Temporary disable this functionality because of wrong path override
        public static void UpdatePath(string variable)
        {
            try
            {
                var name = "PATH";
                var pathVariable = Environment.GetEnvironmentVariable(name);
                var newPathVariable = pathVariable + (pathVariable.EndsWith(";") ? string.Empty : ";") + $@"%{variable}%";

                //if (!pathvar.Contains(desticationFolder) && !pathvar.Contains(variable))
                    //Environment.SetEnvironmentVariable(newPathVariable, name, EnvironmentVariableTarget.Machine);
            }
            catch (Exception ex)
            {
                hLog.Error(ex, "Error occurred during updating PATH environment variable");
                throw new WebDriverManagerException("Error occurred during updating PATH environment variable", ex);
            }
        }

        /// <summary>
        /// Install application from file
        /// </summary>
        /// <param name="command">Installation command</param>
        public static void Install(string command)
        {
            try
            {
                if (File.Exists(desticationFile) && isNew)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = desticationFile,
                        Arguments = command
                    };
                    Process process = new Process
                    {
                        StartInfo = startInfo
                    };
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                hLog.Error(ex, $"Error occurred during application installation from file '{desticationFile}' using command '{command}'");
                throw new WebDriverManagerException($"Error occurred during application installation from file '{desticationFile}' using command '{command}'", ex);
            }
        }
    }
}
