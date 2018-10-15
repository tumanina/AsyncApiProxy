namespace AsyncApiProxy.Api.Configuration
{
    public class QueueConfiguration
    {
        public int Type { get; set; }
        public ServerConfiguration Server { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
    }
}
