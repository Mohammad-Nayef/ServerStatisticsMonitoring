namespace SignalREndpoint
{
    public interface IAlertSender
    {
        Task SendAsync(string alert);
    }
}