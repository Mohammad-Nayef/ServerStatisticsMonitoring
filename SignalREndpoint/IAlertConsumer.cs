namespace SignalREndpoint
{
    public interface IAlertConsumer
    {
        Task ConsumeAsync();
    }
}