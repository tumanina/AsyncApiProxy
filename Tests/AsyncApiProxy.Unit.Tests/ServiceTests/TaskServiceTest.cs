using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace AsyncApiProxy.Unit.Tests.ServiceTests
{
    [TestClass]
    public class TaskServiceTest
    {
        private static readonly Mock<ITaskRepository> TaskRepository = new Mock<ITaskRepository>();

        [TestMethod]
        public void GetTask_TaskExisted_ShouldReturnCorrect()
        {
            TaskRepository.ResetCalls();

            var id = Guid.NewGuid();
            var typeId = 1;
            var statusId = 1;
            var data = "123455";

            var task = new DAL.Entities.Task { Id = id, Type = typeId, Status = statusId, Data = data };

            TaskRepository.Setup(x => x.GetTask(id)).Returns(task);

            var service = new TaskService(TaskRepository.Object);

            var result = service.GetTask(id);

            TaskRepository.Verify(x => x.GetTask(id), Times.Once);
            Assert.AreEqual((int)result.Type, typeId);
            Assert.AreEqual((int)result.Status, statusId);
            Assert.AreEqual(result.Data, data);
            Assert.AreEqual(result.Id, id);
        }

        [TestMethod]
        public void GetTask_TaskNotExisted_ShouldReturnNull()
        {
            TaskRepository.ResetCalls();

            var id = Guid.NewGuid();

            TaskRepository.Setup(x => x.GetTask(id)).Returns((DAL.Entities.Task)null);

            var service = new TaskService(TaskRepository.Object);

            var result = service.GetTask(id);

            TaskRepository.Verify(x => x.GetTask(id), Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetTaskResult_TaskExisted_ShouldReturnCorrect()
        {
            TaskRepository.ResetCalls();

            var id = Guid.NewGuid();
            var typeId = 1;
            var statusId = 1;
            var data = "123455";
            var result = "ghsghg455fh533";

            var task = new DAL.Entities.Task { Id = id, Type = typeId, Status = statusId, Data = data , Result = result };

            TaskRepository.Setup(x => x.GetTask(id)).Returns(task);

            var service = new TaskService(TaskRepository.Object);

            var taskResult = service.GetTask(id).Result;

            TaskRepository.Verify(x => x.GetTask(id), Times.Once);
            Assert.AreEqual(taskResult, result);
        }

        [TestMethod]
        public void GetTaskResult_TaskNotExisted_ShouldReturnNull()
        {
            TaskRepository.ResetCalls();

            var id = Guid.NewGuid();

            TaskRepository.Setup(x => x.GetTask(id)).Returns((DAL.Entities.Task)null);

            var service = new TaskService(TaskRepository.Object);

            var result = service.GetTask(id);

            TaskRepository.Verify(x => x.GetTask(id), Times.Once);
            Assert.AreEqual(result, null);
        }
    }
}
