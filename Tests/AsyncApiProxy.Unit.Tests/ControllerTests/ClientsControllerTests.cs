using System;
using System.Collections.Generic;
using System.Linq;
using AsyncApiProxy.Api.Areas.V1.Controllers;
using AsyncApiProxy.Api.Areas.V1.Models;
using AsyncApiProxy.Api.Models;
using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AsyncApiProxy.Unit.Tests.ControllerTests
{
    [TestClass]
    public class ClientsControllerTests
    {
        private static readonly Mock<IClientService> ClientService = new Mock<IClientService>();
        
        [TestMethod]
        public void CreateClient_ServiceReturnClient_ReturnOk()
        {
            ClientService.ResetCalls();

            var name = "Ivan Ivanov";
            var email = "ivanov@yandex.ru";
            var clientId = Guid.NewGuid();
            var requestedName = string.Empty;
            var requestedEmail = string.Empty;

            var request = new CreateClientRequest { Name = name, Email = email };

            ClientService.Setup(x => x.CreateClient(It.IsAny<Client>()))
                .Callback<Client>((clientParam) =>
                {
                    requestedName = clientParam.Name;
                    requestedEmail = clientParam.Email;
                })
                .Returns(new BusinessLogic.Models.CreateClientResult { Id = clientId });

            var controller = new ClientsController(ClientService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var actionResult = controller.Post(request);
            var result = actionResult as OkObjectResult;
            var createResult = result.Value as Api.Areas.V1.Models.CreateClientResult;

            ClientService.Verify(x => x.CreateClient(It.IsAny<Client>()), Times.Once);
            Assert.AreEqual(createResult.Id, clientId);
            Assert.AreEqual(requestedName, name);
            Assert.AreEqual(requestedEmail, email);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void CreateClient_ServiceReturnTask_ReturnOk()
        {
            ClientService.ResetCalls();

            var name = "Ivan Ivanov";
            var email = "ivanov@yandex.ru";
            var clientId = Guid.NewGuid();
            var requestedName = string.Empty;
            var requestedEmail = string.Empty;
            var taskId = Guid.NewGuid();

            var request = new CreateClientRequest { Name = name, Email = email };

            ClientService.Setup(x => x.CreateClient(It.IsAny<Client>()))
                .Callback<Client>((clientParam) =>
                {
                    requestedName = clientParam.Name;
                    requestedEmail = clientParam.Email;
                })
                .Returns(new BusinessLogic.Models.CreateClientResult { TaskId = taskId });

            var controller = new ClientsController(ClientService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            
            var actionResult = controller.Post(request);
            var result = actionResult as OkObjectResult;
            var createResult = result.Value as TimeOutResult;

            ClientService.Verify(x => x.CreateClient(It.IsAny<Client>()), Times.Once);
            Assert.AreEqual(requestedName, name);
            Assert.AreEqual(requestedEmail, email);
            Assert.AreEqual(createResult.TaskId, taskId);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void CreateClient_ServiceReturnException_ReturnInternalServiceError()
        {
            ClientService.ResetCalls();

            var name = "Ivan Ivanov";
            var email = "ivanov@yandex.ru";
            var requestedName = string.Empty;
            var requestedEmail = string.Empty;
            var exceptionMessage = "some exception message";

            var request = new CreateClientRequest { Name = name, Email = email };

            ClientService.Setup(x => x.CreateClient(It.IsAny<Client>()))
                .Callback<Client>((clientParam) =>
                {
                    requestedName = clientParam.Name;
                    requestedEmail = clientParam.Email;
                })
                .Throws(new Exception(exceptionMessage));

            var controller = new ClientsController(ClientService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };

            var actionResult = controller.Post(request);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            ClientService.Verify(x => x.CreateClient(It.IsAny<Client>()), Times.Once);
            Assert.AreEqual(requestedName, name);
            Assert.AreEqual(requestedEmail, email);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }
    }
}
