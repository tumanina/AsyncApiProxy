using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace ApiProxy.Unit.Tests
{
    [TestClass]
    public class ClientsServiceTest
    {
        private static readonly Mock<IRequestManager> RequestManager = new Mock<IRequestManager>();

        [TestMethod]
        public void CreateClient_ResponseCaught_ShouldReturnId()
        {
            RequestManager.ResetCalls();

            var taskId = Guid.NewGuid();
            var name = "test client 1";
            var email = "test client email 1";
            var data = string.Empty;
            var clientId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var callbackQueueName = taskId + "_" + "CallbackQueue";
            var callbackQueueName1 = "";
            var type = "";
            var message = "";
            
            RequestManager.Setup(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((typeParam, messageParam, callbackQueueNameParam) =>
                {
                    type = typeParam;
                    callbackQueueName1 = callbackQueueNameParam;
                    message = messageParam;
                })
            .Returns(new RequestResult { Result = true, Value = JsonConvert.SerializeObject(new { clientId })});
            
            var service = new ClientsService(RequestManager.Object);

            var result = service.CreateClient(new Cliient { Name= name, Email = email });

            NodeService.Verify(x => x.GetHotNodeId(currency), Times.Once);
            RequestManager.Verify(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.Id, clientId);
            Assert.IsFalse(result.TaskId.HasValue);
            Assert.IsTrue(message.Contains(callbackQueueName));
            Assert.IsTrue(message.Contains(taskId.ToString()));
            Assert.IsTrue(message.Contains(name));
            Assert.IsTrue(message.Contains(email));
            Assert.AreEqual(type, MessageType.Clients.ToString());
            Assert.AreEqual(callbackQueueName1, callbackQueueName);
            Assert.IsFalse(data.Contains(callbackQueueName));
            Assert.IsFalse(data.Contains(taskId.ToString()));
            Assert.IsTrue(data.Contains(name));
            Assert.IsTrue(data.Contains(email));
        }

        [TestMethod]
        public void CreateAddress_ResponseNotCaught_ShouldReturnTaskId()
        {
            RequestManager.ResetCalls();

            var taskId = 1256;
            var taskTypeId = 1;
            var taskStatus = 0;
            var nodeId = 2;
            var label = "test address 1";
            var data = string.Empty;
            var date = DateTime.UtcNow;
            var currency = "BIT";
            var callbackQueueName = taskId + "_" + "CallbackQueue";
            var type = "";
            var callbackQueueName1 = "";
            var message = "";

            RequestManager.Setup(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((typeParam, messageParam, callbackQueueNameParam) =>
                {
                    type = typeParam;
                    callbackQueueName1 = callbackQueueNameParam;
                    message = messageParam;
                })
            .Returns(new RequestResult { Result = false });

            var service = new AddressService(AddressRepository.Object, TaskRepository.Object, NodeService.Object, RequestManager.Object);

            var result = service.CreateAddress(currency, label);

            RequestManager.Verify(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsTrue(string.IsNullOrEmpty(result.Address));
            Assert.IsTrue(result.TaskId.HasValue);
            Assert.AreEqual(result.TaskId, taskId);
            Assert.IsTrue(message.Contains(callbackQueueName));
            Assert.AreEqual(type, TaskType.CreateAddress.ToString());
            Assert.AreEqual(callbackQueueName1, callbackQueueName);
            Assert.AreEqual(taskTypeId, (int)TaskType.CreateAddress);
            Assert.AreEqual(taskStatus, (int)TaskStatus.Created);
            Assert.IsFalse(data.Contains(callbackQueueName));
        }
    }
}
