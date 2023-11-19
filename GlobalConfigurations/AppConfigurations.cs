using System.Text.Json;
using GlobalConfigurations.Models;

namespace GlobalConfigurations
{
    public class AppConfigurations
    {
        public ServerStatisticsConfigDTO ServerStatisticsConfig { get; set; }
        public AnomalyDetectionConfigDTO AnomalyDetectionConfig { get; set; }
        public SignalRConfigDTO SignalRConfig { get; set; }
        public MongoDbConfigDTO MongoDbConfig { get; set; }
        public RabbitMqConfigDTO RabbitMqConfig { get; set; }

        public AppConfigurations()
        {
            var settings = GetDeserializedSettings();
            ServerStatisticsConfig = settings.ServerStatisticsConfig;
            AnomalyDetectionConfig = settings.AnomalyDetectionConfig;
            SignalRConfig = settings.SignalRConfig;
            MongoDbConfig = settings.MongoDbConfig;
            RabbitMqConfig = settings.RabbitMqConfig;
        }

        private AppSettingsDTO? GetDeserializedSettings()
        {
            var json = File.ReadAllText(
                @"C:\repos\ServerMonitoringSolution\GlobalConfigurations\appsettings.json");
            var settings = JsonSerializer.Deserialize<AppSettingsDTO>(json);

            return settings;
        }
    }
}
