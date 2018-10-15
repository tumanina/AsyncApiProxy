using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Unit.Tests
{
    [TestClass]
    public class RequestManagerTest
    {
        private static readonly Mock<IConnectionFactory> ConnectionFactory = new Mock<IConnectionFactory>();
        private static readonly Mock<IConnection> Connection = new Mock<IConnection>();
        private static readonly Mock<IModel> Model = new Mock<IModel>();
        private static readonly Mock<ISenderProcessor> SenderProcessor = new Mock<ISenderProcessor>();
        private static readonly Mock<ISubscriptionFactory> SubsctiptionFactory = new Mock<ISubscriptionFactory>();
        private static readonly Mock<ISubscription> Subscription = new Mock<ISubscription>();

        [TestMethod]
        public void TryToExecute_ResponseCaught_ShouldReturnTrueAndValue()
        {
            ConnectionFactory.ResetCalls();
            SenderProcessor.ResetCalls();

            var message = "{ test }";
            var requestResult = "test result";
            var queueName = "126_queue";
            var timeToWait = 5000;
            var callbackQueueName = string.Empty;
            var type = string.Empty;
            var queueName1 = string.Empty;
            var message1 = string.Empty;
            bool? autoAck = null;
            bool? durable = null;
            bool? exclusive = null;
            bool? autoDelete = null;

            var args = new BasicDeliverEventArgs { Body = Encoding.UTF8.GetBytes(requestResult) };

            Subscription.Setup(x => x.Next(It.IsAny<int>(), out args))
                .Returns(true);

            SubsctiptionFactory.Setup(x => x.CreateSubscription(It.IsAny<IModel>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Callback<IModel, string, bool>((channelParam, callbackQueueNameParam, autoAckParam) =>
                {
                    callbackQueueName = callbackQueueNameParam;
                    autoAck = autoAckParam;
                })
                .Returns(Subscription.Object);

            Model.Setup(x => x.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null))
                .Callback<string, bool, bool, bool, IDictionary<string, object>>((queueParam, durableParam, exclusiveParam, autoDeleteParam, param) =>
                {
                    queueName1 = queueParam;
                    durable = durableParam;
                    exclusive = exclusiveParam;
                    autoDelete = autoDeleteParam;
                });
            Connection.Setup(x => x.CreateModel()).Returns(Model.Object);
            ConnectionFactory.Setup(x => x.CreateConnection()).Returns(Connection.Object);

            SenderProcessor.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((typeParam, messageParam) =>
                {
                    type = typeParam;
                    message1 = messageParam;
                });

            var service = new RequestManager(ConnectionFactory.Object, SenderProcessor.Object, SubsctiptionFactory.Object, timeToWait);

            var taskType = "test";
            var result = service.TryToExecute(taskType, message, queueName);

            ConnectionFactory.Verify(x => x.CreateConnection(), Times.Once);
            Connection.Verify(x => x.CreateModel(), Times.Once);
            Connection.Verify(x => x.Close(), Times.Once);
            Model.Verify(x => x.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null), Times.Once);
            Connection.Verify(x => x.Close(), Times.Once);
            SenderProcessor.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            SubsctiptionFactory.Verify(x => x.CreateSubscription(It.IsAny<IModel>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            Subscription.Verify(x => x.Next(It.IsAny<int>(), out args), Times.Once);
            Assert.AreEqual(result.Value, requestResult);
            Assert.IsTrue(result.Result);
            Assert.AreEqual(type, taskType);
            Assert.AreEqual(message1, message);
            Assert.AreEqual(callbackQueueName, queueName);
            Assert.AreEqual(queueName1, queueName);
            Assert.IsFalse(autoAck.Value);
            Assert.IsFalse(durable.Value);
            Assert.IsFalse(exclusive.Value);
            Assert.IsTrue(autoDelete.Value);
            Assert.AreEqual(autoAck, false);
            Assert.AreEqual(durable, false);
            Assert.AreEqual(exclusive, false);
            Assert.AreEqual(autoDelete, true);
        }
    }
}
