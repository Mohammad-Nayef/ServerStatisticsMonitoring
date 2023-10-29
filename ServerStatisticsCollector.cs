using System.Diagnostics;

namespace ServerMonitoring
{
    public class ServerStatisticsCollector
    {
        private PerformanceCounter _cpuCounter;

        public ServerStatisticsCollector()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        private double GetCurrentCpuUsage() => _cpuCounter.NextValue();
    }
}
