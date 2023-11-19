using GlobalConfigurations;
using ServerStatistics.Models;

namespace ServerStatistics.Extensions
{
    public static class AnomaliesDetection
    {
        private static AppConfigurations Config = new();

        public static bool HasSuddenMemoryUsageIncrease(
            this ServerStatisticsDTO serverStatistics,
            ServerStatisticsDTO previousStatistics)
        {
            var safeMemoryUsageLimit = previousStatistics.MemoryUsage *
                (1 + Config.AnomalyDetectionConfig.MemoryUsageAnomalyThresholdPercentage);

            if (serverStatistics.MemoryUsage > safeMemoryUsageLimit)
                return true;

            return false;
        }

        public static bool HasSuddenCpuUsageIncrease(
            this ServerStatisticsDTO serverStatistics,
            ServerStatisticsDTO previousStatistics)
        {
            var safeCpuUsageLimit = previousStatistics.CpuUsage *
                (1 + Config.AnomalyDetectionConfig.CpuUsageAnomalyThresholdPercentage);

            if (serverStatistics.CpuUsage > safeCpuUsageLimit)
                return true;

            return false;
        }

        public static bool MemoryUsageExceededThreshold(this ServerStatisticsDTO serverStatistics)
        {
            var memoryUsagePercentage = serverStatistics.MemoryUsage /
                (serverStatistics.MemoryUsage + serverStatistics.AvailableMemory);

            if (memoryUsagePercentage >
                Config.AnomalyDetectionConfig.MemoryUsageThresholdPercentage)
                return true;

            return false;
        }

        public static bool CpuUsageExceededThreshold(this ServerStatisticsDTO serverStatistics)
        {
            if (serverStatistics.CpuUsage >
                Config.AnomalyDetectionConfig.CpuUsageThresholdPercentage)
                return true;

            return false;
        }
    }
}
