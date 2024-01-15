namespace ServerStatistics.Models
{
    public class ServerStatisticsDTO
    {
        public double MemoryUsage { get; set; }
        public double AvailableMemory { get; set; }
        public double CpuUsage { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"\nMemory Usage: {MemoryUsage * 100} %\n" +
                   $"Available Memory: {AvailableMemory}\n" +
                   $"CPU Usage: {CpuUsage * 100} %\n" +
                   $"Timestamp: {Timestamp}";
        }
    }
}
