using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MessageBroker.Unit.Tests
{
    [TestClass]
    public class SenderProcessorTest
    {
        [TestMethod]
        public void SendMessage_SenderExist_ShouldUseCorrectSender()
        {
            var message = "test message";
            var type = "test";

            var sendingMessage = string.Empty;

            var sender = new Mock<ISender>();
            sender.Setup(x => x.Type).Returns("test");
            sender.Setup(x => x.SendMessage(message)).Callback((string messageParam) => { sendingMessage = messageParam; });

            var processor = new SenderProcessor(new List<ISender> { sender.Object });

            processor.SendMessage(type, message);

            sender.Verify(x => x.SendMessage(message), Times.Once);
            sender.Verify(x => x.Type, Times.Once);
            Assert.AreEqual(sendingMessage, message);
        }

        [TestMethod]
        public void SendMessage_SenderNotExisted_ThrowException()
        {
            var message = "test message";
            var type = "test1";

            var sendingMessage = string.Empty;

            var sender = new Mock<ISender>();
            sender.Setup(x => x.Type).Returns("test");
            sender.Setup(x => x.SendMessage(message)).Callback((string messageParam) => { sendingMessage = messageParam; });

            var processor = new SenderProcessor(new List<ISender> { sender.Object });

            try
            {
                processor.SendMessage(type, message);

                Assert.Fail();
            }
            catch (Exception ex)
            {
                sender.Verify(x => x.SendMessage(message), Times.Never);
                sender.Verify(x => x.Type, Times.Once);
                Assert.AreEqual(sendingMessage, string.Empty);
                Assert.AreEqual(ex.Message, $"Sender for type '{type}' not found.");
            }
        }
    }
}
