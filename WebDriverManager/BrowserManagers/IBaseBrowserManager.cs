namespace WebDriverManager.BrowserManagers
{
    public interface IBaseBrowserManager
    {
        /// <summary>
        /// Get latest browser driver version number
        /// </summary>
        /// <returns>Version</returns>
        string GetLatestVersion();

        /// <summary>
        /// Get browser driver with default binaries destination folder ([current working directory]\Drivers)
        /// </summary>
        void Init();

        /// <summary>
        /// Get browser driver with custom binaries destination folder
        /// </summary>
        /// <param name="destination">Custom binaries destination folder path</param>
        void Init(string destination);

        /// <summary>
        /// Basic browser driver initializing workflow
        /// </summary>
        void Base();
    }
}