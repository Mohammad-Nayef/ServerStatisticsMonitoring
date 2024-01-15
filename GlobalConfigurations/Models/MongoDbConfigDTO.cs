namespace GlobalConfigurations.Models
{
    public class MongoDbConfigDTO
    {
        public string ConnectionString { get; set; }
        public string ServerMonitoringDatabaseName { get; set; }
        public string ServerStatisticsCollectionName { get; set; }
    }
}
