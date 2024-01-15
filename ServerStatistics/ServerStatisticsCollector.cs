using System.Diagnostics;
using ServerStatistics.Models;

namespace ServerStatistics
{
    public class ServerStatisticsCollector
    {
        private ServerStatisticsDTO _currentServerStatistics;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private Process _currentProcess;
        private bool _firstCall = true;

        public ServerStatisticsDTO CurrentServerStatistics
        {
            get
            {
                _currentServerStatistics.Timestamp = DateTime.Now;
                _currentServerStatistics.CpuUsage = GetCurrentCpuUsage();
                _currentServerStatistics.AvailableMemory = GetCurrentAvailableMemory();
                _currentServerStatistics.MemoryUsage = GetCurrentMemoryUsage();

                return _currentServerStatistics;
            }
        }

        public ServerStatisticsCollector()
        {
            _currentServerStatistics = new ServerStatisticsDTO();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            _currentProcess = Process.GetCurrentProcess();
        }

        private double GetCurrentCpuUsage() => _cpuCounter.NextValue() / 100;

        private double GetCurrentAvailableMemory() => _ramCounter.NextValue();

        private double GetCurrentMemoryUsage()
        {
            if (_firstCall)
            {
                _firstCall = false;
            }
            else
            {
                _currentProcess.Refresh();
            }

            return GetMemoryUsagePercent();
        }

        private double GetMemoryUsagePercent()
        {
            var usedMemory = ToMegaBytes(_currentProcess.WorkingSet64);

            return usedMemory / (usedMemory + GetCurrentAvailableMemory());
        }

        private double ToMegaBytes(long workingSet64) => workingSet64 / (1024 * 1024);
    }
}
