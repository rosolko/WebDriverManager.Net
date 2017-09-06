using System.Diagnostics;
using System.Threading;

namespace IntegrationTests.BrowserTests
{
    public static class Helper
    {
        public static void KillProcesses(string driverExe)
        {
            Thread.Sleep(1500);
            var driverProcesses = Process.GetProcessesByName(driverExe);
            foreach (var driverProcess in driverProcesses)
            {
                driverProcess.Kill();
            }
        }
    }
}