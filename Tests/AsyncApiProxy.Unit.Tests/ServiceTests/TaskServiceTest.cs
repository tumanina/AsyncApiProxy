using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.BusinessLogic.Models;
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

        [TestMethod]
        public void CreateTask_Correct_ShouldTask()
        {
            TaskRepository.ResetCalls();

            var taskId = Guid.NewGuid();
            var taskTypeId = 0;
            var taskStatus = 0;
            var data = string.Empty;
            var date = DateTime.UtcNow;

            TaskRepository.Setup(x => x.CreateTask(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback((int taskTypeParam, int statusParam, string dataParam) =>
                {
                    taskTypeId = taskTypeParam;
                    taskStatus = statusParam;
                    data = dataParam;
                })
                .Returns(new DAL.Entities.Task { Id = taskId, Status = (int)TaskStatus.Created, CreatedDate = date, UpdatedDate = date });

            var service = new TaskService(TaskRepository.Object);
            var result = service.CreateTask(TaskType.CreateClient, "name: test");

            TaskRepository.Verify(x => x.CreateTask(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.Id, taskId);
            Assert.AreEqual(taskTypeId, (int)TaskType.CreateClient);
            Assert.AreEqual(taskStatus, (int)TaskStatus.Created);
            Assert.IsFalse(data.Contains(taskId.ToString()));
        }
    }
}
