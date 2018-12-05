using System;
using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.BusinessLogic.Models;
using MessageBroker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace AsyncApiProxy.Unit.Tests
{
    [TestClass]
    public class ClientServiceTest
    {
        private static readonly Mock<ITaskService> TaskService = new Mock<ITaskService>();
        private static readonly Mock<IRequestManager> RequestManager = new Mock<IRequestManager>();

        [TestMethod]
        public void CreateClient_ResponseCaught_ShouldReturnId()
        {
            ResetCalls();

            var name = "Ivan Ivanov";
            var email = "ivanov@yandex.ru";
            var password = "123654";
            var taskId = Guid.NewGuid();
            var taskTypeId = 0;
            var data = string.Empty;
            var date = DateTime.UtcNow;
            var clienId = Guid.NewGuid();
            var callbackQueueName = taskId + "_" + "CallbackQueue";
            var type = String.Empty;
            var callbackQueueName1 = "";
            var message = "";

            TaskService.Setup(x => x.CreateTask(It.IsAny<TaskType>(), It.IsAny<string>()))
                .Callback((TaskType taskTypeParam, string dataParam) =>
                {
                    taskTypeId = (int)taskTypeParam;
                    data = dataParam;
                })
                .Returns(new Task {Id = taskId, Status = TaskStatus.Created });
            
            RequestManager.Setup(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((typeParam, messageParam, callbackQueueNameParam) =>
                {
                    type = typeParam;
                    callbackQueueName1 = callbackQueueNameParam;
                    message = messageParam;
                })
                .Returns(new RequestResult { Result = true, Value = JsonConvert.SerializeObject(new { Id = clienId })});
            
            var service = new ClientService(TaskService.Object, RequestManager.Object);
            var result = service.CreateClient(new Client { Name = name, Email = email, Password = password });

            TaskService.Verify(x => x.CreateTask(It.IsAny<TaskType>(), It.IsAny<string>()), Times.Once);
            RequestManager.Verify(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.Id, clienId);
            Assert.IsFalse(result.TaskId.HasValue);
            Assert.IsTrue(message.Contains(callbackQueueName));
            Assert.IsTrue(message.Contains(taskId.ToString()));
            Assert.AreEqual(type, MessageType.CreateClient.ToString());
            Assert.AreEqual(callbackQueueName1, callbackQueueName);
            Assert.AreEqual(taskTypeId, (int)TaskType.CreateClient);
            Assert.IsFalse(data.Contains(callbackQueueName));
            Assert.IsFalse(data.Contains(taskId.ToString()));
        }

        [TestMethod]
        public void CreateClient_ResponseNotCaught_ShouldReturnTaskId()
        {
            ResetCalls();

            var name = "Ivan Ivanov";
            var email = "ivanov@yandex.ru";
            var password = "123654";
            var taskId = Guid.NewGuid();
            var taskTypeId = 0;
            var data = string.Empty;
            var date = DateTime.UtcNow;
            var clienId = Guid.NewGuid();
            var callbackQueueName = taskId + "_" + "CallbackQueue";
            var type = String.Empty;
            var callbackQueueName1 = "";
            var message = "";

            TaskService.Setup(x => x.CreateTask(It.IsAny<TaskType>(), It.IsAny<string>()))
                .Callback((TaskType taskTypeParam, string dataParam) =>
                {
                    taskTypeId = (int)taskTypeParam;
                    data = dataParam;
                })
                .Returns(new Task { Id = taskId, Status = TaskStatus.Created });

            RequestManager.Setup(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((typeParam, messageParam, callbackQueueNameParam) =>
                {
                    type = typeParam;
                    callbackQueueName1 = callbackQueueNameParam;
                    message = messageParam;
                })
            .Returns(new RequestResult { Result = false });

            var service = new ClientService(TaskService.Object, RequestManager.Object);
            var result = service.CreateClient(new Client { Name = name, Email = email, Password = password });

            TaskService.Verify(x => x.CreateTask(It.IsAny<TaskType>(), It.IsAny<string>()), Times.Once);
            RequestManager.Verify(x => x.TryToExecute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.IsNull(result.Id);
            Assert.IsTrue(result.TaskId.HasValue);
            Assert.AreEqual(result.TaskId, taskId);
            Assert.IsTrue(message.Contains(callbackQueueName));
            Assert.AreEqual(type, MessageType.CreateClient.ToString());
            Assert.AreEqual(callbackQueueName1, callbackQueueName);
            Assert.AreEqual(taskTypeId, (int)TaskType.CreateClient);
            Assert.IsFalse(data.Contains(callbackQueueName));
        }

        private void ResetCalls()
        {
            TaskService.Invocations.Clear();
            RequestManager.Invocations.Clear();
        }
    }
}
