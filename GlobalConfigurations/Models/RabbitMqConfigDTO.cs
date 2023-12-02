namespace GlobalConfigurations.Models
{
    public class RabbitMqConfigDTO
    {
        public string HostName { get; set; }
        public string ConsumerConnectionName { get; set; }
        public string PublisherConnectionName { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public double ReconnectIntervalSeconds { get; set; }
    }
}
