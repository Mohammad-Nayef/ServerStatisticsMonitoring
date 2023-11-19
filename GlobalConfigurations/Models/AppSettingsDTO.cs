namespace GlobalConfigurations.Models
{
    public class AppSettingsDTO
    {
        public ServerStatisticsConfigDTO ServerStatisticsConfig { get; set; }
        public AnomalyDetectionConfigDTO AnomalyDetectionConfig { get; set; }
        public SignalRConfigDTO SignalRConfig { get; set; }
        public MongoDbConfigDTO MongoDbConfig { get; set; }
        public RabbitMqConfigDTO RabbitMqConfig { get; set; }
    }
}
