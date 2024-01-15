using GlobalConfigurations.Models;

namespace GlobalConfigurations
{
    public interface IAppConfigurations
    {
        AnomalyDetectionConfigDTO AnomalyDetectionConfig { get; set; }
        MongoDbConfigDTO MongoDbConfig { get; set; }
        RabbitMqConfigDTO RabbitMqConfig { get; set; }
        ServerStatisticsConfigDTO ServerStatisticsConfig { get; set; }
        SignalRConfigDTO SignalRConfig { get; set; }
    }
}