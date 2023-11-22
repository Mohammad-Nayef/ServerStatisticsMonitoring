using GlobalConfigurations;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalREndpoint
{
    public class SignalRAlertConsumer : IAlertConsumer
    {
        public HubConnection Connection { get; private set; }

        public SignalRAlertConsumer()
        {
            var config = new AppConfigurations();

            Connection = new HubConnectionBuilder()
                .WithUrl(config.SignalRConfig.Url)
                .Build();
        }

        public async Task ConsumeAsync()
        {
            Connection.On<string>("ReceiveAlerts", alert =>
            {
                Console.WriteLine(alert);
            });

            await Connection.StartAsync();
        }

        public async Task ReconnectAsync()
        {
            await Connection.StartAsync();
        }
    }
}
