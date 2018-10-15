using System;
using System.Text;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class RequestManager: IRequestManager
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ISenderProcessor _senderProcessor;
        private readonly ISubscriptionFactory _subsctiptionFactory;
        private readonly int _timeToWait;

        public RequestManager(IConnectionFactory connectionFactory, ISenderProcessor senderProcessor, ISubscriptionFactory subsctiptionFactory, int timeToWait)
        {
            _connectionFactory = connectionFactory;
            _senderProcessor = senderProcessor;
            _subsctiptionFactory = subsctiptionFactory;
            _timeToWait = timeToWait;
        }

        public RequestResult TryToExecute(string type, string message, string callbackQueueName)
        {
            var connection = _connectionFactory.CreateConnection();

            try
            {
                var channel = connection.CreateModel();

                channel.ExchangeDeclare("amq.direct", ExchangeType.Direct, true);
                channel.QueueDeclare(callbackQueueName, false, false, true, null);
                channel.QueueBind(callbackQueueName, "amq.direct", callbackQueueName, null);

                _senderProcessor.SendMessage(type, message);

                var subscription = _subsctiptionFactory.CreateSubscription(channel, callbackQueueName, false);

                var result = subscription.Next(_timeToWait, out var basicDeliveryEventArgs);
                if (result)
                {
                    var messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
                    subscription.Ack(basicDeliveryEventArgs);

                    connection.Close();
                    return new RequestResult { Result = true, Value = messageContent };
                }
                else
                {
                    connection.Close();
                    return new RequestResult { Result = false };
                }
            }
            catch (Exception e)
            {
                connection.Close();
                throw;
            }
        }
    }

    public class RequestResult
    {
        public bool Result { get; set; }
        public string Value { get; set; }
    }
}
