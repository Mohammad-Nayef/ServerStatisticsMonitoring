using System.Diagnostics;

namespace ServerMonitoring
{
    public class ServerStatisticsCollector
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private Process _currentProcess;

        public ServerStatisticsCollector()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            _currentProcess = Process.GetCurrentProcess();
        }

        private double GetCurrentCpuUsage() => _cpuCounter.NextValue();

        private double GetCurrentAvailableMemory() => _ramCounter.NextValue();

        private double GetCurrentMemoryUsage() => _currentProcess.WorkingSet64 / (1024 * 1024);
    }
}
