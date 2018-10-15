using MessageBroker;
using RabbitMQ.Client;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public class MessageService : IMessageService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ISenderProcessor _senderProcessor;
        private readonly ISubscriptionFactory _subsctiptionFactory;
        private readonly int _timeToWait;

        public MessageService(IConnectionFactory connectionFactory, ISenderProcessor senderProcessor, ISubscriptionFactory subsctiptionFactory, int timeToWait)
        {
            _connectionFactory = connectionFactory;
            _senderProcessor = senderProcessor;
            _subsctiptionFactory = subsctiptionFactory;
            _timeToWait = timeToWait;
        }

        public void SendMessage(string message)
        {
            var connection = _connectionFactory.CreateConnection();

            try
            {
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("amq.direct", ExchangeType.Direct, true);

                _senderProcessor.SendMessage("test", message);
                connection.Close();
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }
    }
}
