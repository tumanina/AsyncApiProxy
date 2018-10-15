namespace AsyncApiProxy.BusinessLogic
{
    public interface IMessageService
    {
        Result SendMessage(string message);
    }
}
