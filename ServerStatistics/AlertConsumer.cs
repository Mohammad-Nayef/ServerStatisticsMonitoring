using Microsoft.AspNetCore.SignalR.Client;

namespace ServerStatistics
{
    public class AlertConsumer
    {
        public async Task ConsumeAsync()
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7265/Alerts")
                .Build();

            hubConnection.On<string>("ReceiveAlerts", alert =>
            {
                Console.WriteLine(alert);
            });

            await hubConnection.StartAsync();
        }
    }
}
