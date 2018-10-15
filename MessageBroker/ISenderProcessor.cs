namespace MessageBroker
{
    public interface ISenderProcessor
    {
        void SendMessage(string type, string message);
    }
}
