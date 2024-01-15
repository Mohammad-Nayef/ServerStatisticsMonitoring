using Microsoft.AspNetCore.SignalR;

namespace ServerMonitoring.Hubs
{
    public class AlertsHub : Hub
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("ReceiveAlerts", message);
        }
    }
}
