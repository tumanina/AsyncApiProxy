using AsyncApiProxy.Api.Areas.V1.Controllers;
using AsyncApiProxy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Task = AsyncApiProxy.BusinessLogic.Models.Task;

namespace AsyncApiProxy.Unit.Tests.ControllerTests
{
    [TestClass]
    public class TasksControllerTests
    {
        private static readonly Mock<ITaskService> TaskService = new Mock<ITaskService>();

        [TestMethod]
        public void GetTask_TaskExisted_ReturnOk()
        {
            TaskService.ResetCalls();

            var id = Guid.NewGuid();
            var type = 1;
            var status = 1;
            var data = "123457";
            var taskResult = "jfghyfgt6747";

            TaskService.Setup(x => x.GetTask(id)).Returns(new Task { Id = id, Type = type, Status = status, Data = data, Result = taskResult });

            var controller = new TasksController(TaskService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var task = result.Value as Api.Areas.V1.Models.Task;

            TaskService.Verify(x => x.GetTask(id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(task.Type, type);
            Assert.AreEqual(task.Status, status);
            Assert.AreEqual(task.Result, taskResult);
        }
        
        [TestMethod]
        public void GetTask_TaskNotExisted_ReturnNotFound()
        {
            TaskService.ResetCalls();

            var id = Guid.NewGuid();

            TaskService.Setup(x => x.GetTask(id)).Returns((Task) null);

            var controller = new TasksController(TaskService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            TaskService.Verify(x => x.GetTask(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetTask_ServiceReturnException_ReturnInternalServerError()
        {
            TaskService.ResetCalls();

            var id = Guid.NewGuid();
            var exceptionMessage = "some exception message";

            TaskService.Setup(x => x.GetTask(id)).Throws(new Exception(exceptionMessage));

            var controller = new TasksController(TaskService.Object);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            TaskService.Verify(x => x.GetTask(id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }
    }
}
