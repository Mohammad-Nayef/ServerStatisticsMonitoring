namespace MessageQueue
{
    public interface IMessageSender
    {
        void Send<Type>(Type message);
    }
}
