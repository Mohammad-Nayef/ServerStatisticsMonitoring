using System.Diagnostics;

namespace ServerMonitoring
{
    public class ServerStatisticsCollector
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;

        public ServerStatisticsCollector()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private double GetCurrentCpuUsage() => _cpuCounter.NextValue();

        private double GetCurrentAvailableMemory() => _ramCounter.NextValue();
    }
}
