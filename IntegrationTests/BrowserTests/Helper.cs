using System.Diagnostics;

namespace IntegrationTests.BrowserTests
{
    public static class Helper
    {
        public static void KillProcesses(string driverExe)
        {
            var driverProcesses = Process.GetProcessesByName(driverExe);
            foreach (var driverProcess in driverProcesses)
            {
                driverProcess.Kill();
            }
        }
    }
}