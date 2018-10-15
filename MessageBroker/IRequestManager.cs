namespace MessageBroker
{
    public interface IRequestManager
    {
        RequestResult TryToExecute(string type, string message, string callbackQueueName);
    }
}
