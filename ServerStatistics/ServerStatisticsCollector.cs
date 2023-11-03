using System.Diagnostics;

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

        private double GetCurrentCpuUsage() => _cpuCounter.NextValue();

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

            return ToMegaBytes(_currentProcess.WorkingSet64);
        }

        private double ToMegaBytes(long workingSet64) => workingSet64 / (1024 * 1024);
    }
}
