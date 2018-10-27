using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;

namespace MessageBroker.UnitTests
{
    [TestClass]
    public class SubscriptionFactoryTest
    {
        [TestMethod]
        public void CreateSubscription_SenderExist_ShouldUseCorrectSender()
        {
            var queueName = "126_queue";
            var autoAck = false;
            var model = new Mock<IModel>();

            var factory = new SubscriptionFactory();

            var subscription = factory.CreateSubscription(model.Object, queueName, autoAck);

            Assert.AreEqual(subscription.AutoAck, autoAck);
            Assert.AreEqual(subscription.QueueName, queueName);
            Assert.AreEqual(subscription.Model, model.Object);
        }
    }
}
