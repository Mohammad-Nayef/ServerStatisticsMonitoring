using GlobalConfigurations;
using ServerStatistics.Models;
using SignalREndpoint;

namespace ServerStatistics.Services
{
    public class ServerAnomaliesService : IServerAnomaliesService
    {
        private IAppConfigurations _config;
        private IAlertSender _alertSender;
        private ServerStatisticsDTO _serverStatistics;
        private ServerStatisticsDTO _previousStatistics = null;

        public ServerAnomaliesService(IAppConfigurations config, IAlertSender alertSender)
        {
            _config = config;
            _alertSender = alertSender;
        }

        public async Task DetectAndReportAsync(ServerStatisticsDTO serverStatistics)
        {
            _serverStatistics = serverStatistics;

            if (_previousStatistics == null)
            {
                _previousStatistics = _serverStatistics;
            }
            else
            {
                if (HasSuddenMemoryUsageIncrease())
                    await ReportAsync("Memory usage anomaly alert");

                if (HasSuddenCpuUsageIncrease())
                    await ReportAsync("CPU usage anomaly alert");
            }

            if (MemoryUsageExceededThreshold())
                await ReportAsync("High memory usage alert");

            if (CpuUsageExceededThreshold())
                await ReportAsync("High CPU usage alert");
        }

        private async Task ReportAsync(string alert) => await _alertSender.SendAsync(alert);

        private bool HasSuddenMemoryUsageIncrease()
        {
            var safeMemoryUsageLimit = _previousStatistics.MemoryUsage *
                (1 + _config.AnomalyDetectionConfig.MemoryUsageAnomalyThresholdPercentage);

            if (_serverStatistics.MemoryUsage > safeMemoryUsageLimit)
                return true;

            return false;
        }

        private bool HasSuddenCpuUsageIncrease()
        {
            var safeCpuUsageLimit = _previousStatistics.CpuUsage *
                (1 + _config.AnomalyDetectionConfig.CpuUsageAnomalyThresholdPercentage);

            if (_serverStatistics.CpuUsage > safeCpuUsageLimit)
                return true;

            return false;
        }

        private bool MemoryUsageExceededThreshold()
        {
            if (_serverStatistics.MemoryUsage >
                _config.AnomalyDetectionConfig.MemoryUsageThresholdPercentage)
                return true;

            return false;
        }

        private bool CpuUsageExceededThreshold()
        {
            if (_serverStatistics.CpuUsage >
                _config.AnomalyDetectionConfig.CpuUsageThresholdPercentage)
                return true;

            return false;
        }
    }
}
