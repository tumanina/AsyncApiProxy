using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace MessageBroker
{
    public interface ISubscriptionFactory
    {
        ISubscription CreateSubscription(IModel channel, string callbackQueueName, bool autoAck);
    }
}
