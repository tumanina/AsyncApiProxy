using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace MessageBroker
{
    public class SubscriptionFactory : ISubscriptionFactory
    { 
        public ISubscription CreateSubscription(IModel channel, string callbackQueueName, bool autoAck)
        {
            return new Subscription(channel, callbackQueueName, autoAck);
        }
    }
}
