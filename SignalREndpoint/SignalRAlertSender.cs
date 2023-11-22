using GlobalConfigurations;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalREndpoint
{
    public class SignalRAlertSender : IAlertSender
    {
        private HubConnection? _hubConnection;

        public SignalRAlertSender()
        {
            var config = new AppConfigurations();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(config.SignalRConfig.Url)
                .Build();
        }

        public async Task SendAsync(string alert)
        {
            await _hubConnection.StartAsync();

            await _hubConnection.InvokeAsync<string>("SendMessageAsync", alert);

            await _hubConnection.StopAsync();
        }
    }
}
