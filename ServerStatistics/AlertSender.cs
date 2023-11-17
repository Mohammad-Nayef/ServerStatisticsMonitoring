using Microsoft.AspNetCore.SignalR.Client;

namespace ServerStatistics
{
    public class AlertSender
    {
        public async Task SendAsync(string alert)
        {
            await using var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7265/Alerts")
                .Build();

            await hubConnection.StartAsync();

            await hubConnection.InvokeAsync<string>("SendMessageAsync", alert);
        }
    }
}
