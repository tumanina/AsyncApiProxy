using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultiWallet.Business.Interfaces;
using MultiWallet.Core.Models;
using MultiWallet.WebApi.Areas.V1.Controllers;
using MultiWallet.WebApi.Areas.V1.Models;
using Address = MultiWallet.Business.Models.Address;
using CreateAddressResult = MultiWallet.Business.Models.Results.CreateAddressResult;

namespace ApiProxy.Unit.Tests
{
    [TestClass]
    public class AddressControllerTests
    {
        private static readonly Mock<IAddressService> AddressService = new Mock<IAddressService>();
        private static readonly Mock<ILogger<AddressController>> Logger = new Mock<ILogger<AddressController>>();

        [TestMethod]
        public void GetAll_AddressesExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            var id1 = 126;
            var id2 = 127;
            var id3 = 128;
            var value1 = "hgktb865fjygf";
            var value2 = "hktgh568547gt";
            var value3 = "htpjhjh6556hj";
            var privateKey1 = "4653fgdg5dxgfdh4get14";
            var privateKey2 = "2356fgdg5dxgfdh4get64";
            var privateKey3 = "7345fgdg5dxgfdh4get465";
            var nodeId1 = 12;
            var nodeId2 = 22;
            long clientId = 1;
            var label = "test address 1";
            var amount1 = 456.1245m;
            var amount2 = 12.4512m;
            var amount3 = 145.4512m;

            AddressService.Setup(x => x.GetAddresses(clientId, label, false, 1, 20)).Returns(new PagedList<Address>
            {
                List = new List<Address>
                {
                    new Address { Id = id1, IsUsed = true, NodeId = nodeId1, Value = value1, IsDeleted = false, PrivateKey = privateKey1, ClientId = clientId, Label = label, Amount = amount1 },
                    new Address { Id = id2, IsUsed = true, NodeId = nodeId2, Value = value2, IsDeleted = false, PrivateKey = privateKey2, ClientId = clientId, Label = label, Amount = amount2 },
                    new Address { Id = id3, IsUsed = true, NodeId = nodeId2, Value = value3, IsDeleted = false, PrivateKey = privateKey3, ClientId = clientId, Label = label, Amount = amount3 }
                },
                PageCount = 1,
                PageIndex = 1,
                PageSize = 20,
                TotalCount = 3
            });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAll(label, false, false, 1, 20);
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as PagedList<WebApi.Areas.V1.Models.Address>;

            AddressService.Verify(x => x.GetAddresses(clientId, label, false, 1, 20), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.PageCount, 1);
            Assert.AreEqual(pagedResult.PageIndex, 1);
            Assert.AreEqual(pagedResult.PageSize, 20);
            Assert.AreEqual(pagedResult.TotalCount, 3);
            Assert.AreEqual(pagedResult.List.Count(), 3);
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id1 && t.Value == value1 && t.NodeId == nodeId1 && t.ClientId == clientId && t.Label == label && t.Amount == amount1));
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id2 && t.Value == value2 && t.NodeId == nodeId2 && t.ClientId == clientId && t.Label == label && t.Amount == amount2));
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id3 && t.Value == value3 && t.NodeId == nodeId2 && t.ClientId == clientId && t.Label == label && t.Amount == amount3));
        }

        [TestMethod]
        public void GetAllWithPrivateKeys_AddressesExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            var id1 = 126;
            var id2 = 127;
            var id3 = 128;
            var value1 = "hgktb865fjygf";
            var value2 = "hktgh568547gt";
            var value3 = "htpjhjh6556hj";
            var privateKey1 = "4653fgdg5dxgfdh4get14";
            var privateKey2 = "2356fgdg5dxgfdh4get64";
            var privateKey3 = "7345fgdg5dxgfdh4get465";
            var nodeId1 = 12;
            var nodeId2 = 22;
            long clientId = 1;
            var label = "test address 1";
            var amount1 = 456.1245m;
            var amount2 = 12.4512m;
            var amount3 = 145.4512m;

            AddressService.Setup(x => x.GetAddresses(clientId, label, false, 1, 20)).Returns(new PagedList<Address>
            {
                List = new List<Address>
                {
                    new Address { Id = id1, IsUsed = true, NodeId = nodeId1, Value = value1, IsDeleted = false, PrivateKey = privateKey1, ClientId = clientId, Label = label, Amount = amount1 },
                    new Address { Id = id2, IsUsed = true, NodeId = nodeId2, Value = value2, IsDeleted = false, PrivateKey = privateKey2, ClientId = clientId, Label = label, Amount = amount2 },
                    new Address { Id = id3, IsUsed = true, NodeId = nodeId2, Value = value3, IsDeleted = false, PrivateKey = privateKey3, ClientId = clientId, Label = label, Amount = amount3 }
                },
                PageCount = 1,
                PageIndex = 1,
                PageSize = 20,
                TotalCount = 3
            });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAll(label, false, true, 1, 20);
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as PagedList<AddressWithPrivateKey>;

            AddressService.Verify(x => x.GetAddresses(clientId, label, false, 1, 20), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.PageCount, 1);
            Assert.AreEqual(pagedResult.PageIndex, 1);
            Assert.AreEqual(pagedResult.PageSize, 20);
            Assert.AreEqual(pagedResult.TotalCount, 3);
            Assert.AreEqual(pagedResult.List.Count(), 3);
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id1 && t.Value == value1 && t.NodeId == nodeId1 && t.ClientId == clientId && t.Label == label && t.PrivateKey == privateKey1 && t.Amount == amount1));
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id2 && t.Value == value2 && t.NodeId == nodeId2 && t.ClientId == clientId && t.Label == label && t.PrivateKey == privateKey2 && t.Amount == amount2));
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id3 && t.Value == value3 && t.NodeId == nodeId2 && t.ClientId == clientId && t.Label == label && t.PrivateKey == privateKey3 && t.Amount == amount3));
        }

        [TestMethod]
        public void GetAll_AddressesExisted_ReturnCorrectPagging()
        {
            AddressService.ResetCalls();

            var id = 128;
            var value = "htpjhjh6556hj";
            var nodeId = 3;
            var privateKey = "4653fgdg5dxgfdh4get14";
            long clientId = 1;
            var label = "test address 1";
            var amount = 456.1245m;

            AddressService.Setup(x => x.GetAddresses(clientId, label, false, 2, 2)).Returns(new PagedList<Address>
            {
                List = new List<Address>
                {
                    new Address { Id = id, IsUsed = true, NodeId = nodeId, Value = value, IsDeleted = false, PrivateKey = privateKey, ClientId = clientId, Label = label, Amount = amount }
                },
                PageCount = 2,
                PageIndex = 2,
                PageSize = 2,
                TotalCount = 3
            });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAll(label, false, false, 2, 2);
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as PagedList<WebApi.Areas.V1.Models.Address>;

            AddressService.Verify(x => x.GetAddresses(clientId, label, false, 2, 2), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.PageCount, 2);
            Assert.AreEqual(pagedResult.PageIndex, 2);
            Assert.AreEqual(pagedResult.PageSize, 2);
            Assert.AreEqual(pagedResult.TotalCount, 3);
            Assert.AreEqual(pagedResult.List.Count(), 1);
            Assert.IsTrue(pagedResult.List.Any(t => t.IsUsed == true && t.Id == id && t.Value == value && t.NodeId == nodeId && t.ClientId == clientId && t.Label == label && t.Amount == amount));
        }

        [TestMethod]
        public void GetAll_AddressesNotExisted_ReturnCorrect()
        {
            AddressService.ResetCalls();

            long clientId = 1;
            var label = "test address 1";

            AddressService.Setup(x => x.GetAddresses(clientId, label, false, 1, 20)).Returns(new PagedList<Address>
            {
                List = new List<Address>(),
                PageCount = 0,
                PageIndex = 1,
                PageSize = 20,
                TotalCount = 0
            });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAll(label, false, false, 1, 20);
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as PagedList<WebApi.Areas.V1.Models.Address>;

            AddressService.Verify(x => x.GetAddresses(clientId, label, false, 1, 20), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.PageCount, 0);
            Assert.AreEqual(pagedResult.PageIndex, 1);
            Assert.AreEqual(pagedResult.PageSize, 20);
            Assert.AreEqual(pagedResult.TotalCount, 0);
            Assert.AreEqual(pagedResult.List.Count(), 0);
        }

        [TestMethod]
        public void GetAll_SereviceReturnException_ReturnOk()
        {
            AddressService.ResetCalls();

            var exceptionMessage = "some exception message";
            long clientId = 1;
            var label = "test address 1";

            AddressService.Setup(x => x.GetAddresses(clientId, label, false, 1, 20)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAll(label, false, true, 1, 20);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.GetAddresses(clientId, label, false, 1, 20), Times.Once);
            Assert.AreEqual(result, null);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetAddressById_AddressExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            var id = 126;
            var value = "hgktb865fjygf";
            var nodeId = 1;
            long clientId = 1;

            AddressService.Setup(x => x.GetAddress(clientId, id)).Returns(new Address { Id = id, IsUsed = true, NodeId = nodeId, Value = value });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var pagedResult = result.Value as WebApi.Areas.V1.Models.Address;

            AddressService.Verify(x => x.GetAddress(clientId, id), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(pagedResult.Value, value);
            Assert.AreEqual(pagedResult.Id, id);
            Assert.AreEqual(pagedResult.IsUsed, true);
            Assert.AreEqual(pagedResult.NodeId, nodeId);
        }

        [TestMethod]
        public void GetAddressById_AddressNotExisted_ReturnNotFound()
        {
            AddressService.ResetCalls();

            var id = 126;
            long clientId = 1;

            AddressService.Setup(x => x.GetAddress(clientId, id)).Returns((Address) null);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            AddressService.Verify(x => x.GetAddress(clientId, id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetAddressById_ServiceReturnException_ReturnInternalServerError()
        {
            AddressService.ResetCalls();

            var id = 126;
            long clientId = 1;
            var exceptionMessage = "some exception message";

            AddressService.Setup(x => x.GetAddress(clientId, id)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetById(id);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.GetAddress(clientId, id), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetAddressByValue_AddressExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            var id = 126;
            long clientId = 1;
            var value = "hgktb865fjygf";
            var nodeId = 1;

            AddressService.Setup(x => x.GetAddress(clientId, value)).Returns(new Address { Id = id, IsUsed = true, NodeId = nodeId, Value = value });

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetByValue(value);
            var result = actionResult as OkObjectResult;
            var address = result.Value as WebApi.Areas.V1.Models.Address;

            AddressService.Verify(x => x.GetAddress(clientId, value), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(address.Value, value);
            Assert.AreEqual(address.Id, id);
            Assert.AreEqual(address.IsUsed, true);
            Assert.AreEqual(address.NodeId, nodeId);
        }

        [TestMethod]
        public void GetAddressByValue_AddressNotExisted_ReturnNotFound()
        {
            AddressService.ResetCalls();

            var value = "hgktb865fjygf";
            long clientId = 1;

            AddressService.Setup(x => x.GetAddress(clientId, value)).Returns((Address)null);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetByValue(value);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            AddressService.Verify(x => x.GetAddress(clientId, value), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetAddressByValue_ServiceReturnException_ReturnInternalServerError()
        {
            AddressService.ResetCalls();

            var value = "hgktb865fjygf";
            long clientId = 1;
            var exceptionMessage = "some exception message";

            AddressService.Setup(x => x.GetAddress(clientId, value)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetByValue(value);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.GetAddress(clientId, value), Times.Once);
            Assert.AreEqual(result, null);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetAddressBalance_AddressExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            var address = "hgktb865fjygf";
            long clientId = 1;
            var balance = 456.1245m;

            AddressService.Setup(x => x.GetAddressBalance(clientId, address)).Returns(balance);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAddressBalance(address);
            var result = actionResult as OkObjectResult;

            AddressService.Verify(x => x.GetAddressBalance(clientId, address), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, balance);
        }

        [TestMethod]
        public void GetAddressBalance_AddressNotExisted_ReturnNotFound()
        {
            AddressService.ResetCalls();

            var address = "hgktb865fjygf";
            long clientId = 1;

            AddressService.Setup(x => x.GetAddressBalance(clientId, address)).Returns((decimal?)null);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAddressBalance(address);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as NotFoundResult;

            AddressService.Verify(x => x.GetAddressBalance(clientId, address), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
        }

        [TestMethod]
        public void GetAddressBalance_ServiceReturnException_ReturnInternalServerError()
        {
            AddressService.ResetCalls();

            var address = "hgktb865fjygf";
            long clientId = 1;
            var exceptionMessage = "some exception message";

            AddressService.Setup(x => x.GetAddressBalance(clientId, address)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetAddressBalance(address);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.GetAddressBalance(clientId, address), Times.Once);
            Assert.AreEqual(result, null);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void GetClientBalance_AddressExisted_ReturnOk()
        {
            AddressService.ResetCalls();

            long clientId = 1;
            var balance = 456.1245m;

            AddressService.Setup(x => x.GetBalance(clientId)).Returns(balance);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetBalance();
            var result = actionResult as OkObjectResult;

            AddressService.Verify(x => x.GetBalance(clientId), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, balance);
        }

        [TestMethod]
        public void GetClientBalance_ServiceReturnException_ReturnInternalServerError()
        {
            AddressService.ResetCalls();

            long clientId = 1;
            var exceptionMessage = "some exception message";

            AddressService.Setup(x => x.GetBalance(clientId)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.GetBalance();
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.GetBalance(clientId), Times.Once);
            Assert.AreEqual(result, null);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }

        [TestMethod]
        public void RemoveAddress_Success_ReturnOkAndTrue()
        {
            AddressService.ResetCalls();

            var addressId = 2315;
            long clientId = 1;

            AddressService.Setup(x => x.RemoveAddress(clientId, addressId)).Returns(true);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.Delete(addressId);
            var result = actionResult as OkObjectResult;

            AddressService.Verify(x => x.RemoveAddress(clientId, addressId), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, true);
        }

        [TestMethod]
        public void RemoveAddress_Fail_ReturnOkAndFalse()
        {
            AddressService.ResetCalls();

            var addressId = 2315;
            long clientId = 1;

            AddressService.Setup(x => x.RemoveAddress(clientId, addressId)).Returns(false);

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.Delete(addressId);
            var result = actionResult as OkObjectResult;

            AddressService.Verify(x => x.RemoveAddress(clientId, addressId), Times.Once);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, false);
        }

        [TestMethod]
        public void RemoveAddress_ServiceReturnException_ReturnInternalServerError()
        {
            AddressService.ResetCalls();

            var addressId = 2315;
            long clientId = 1;

            var exceptionMessage = "some exception message";

            AddressService.Setup(x => x.RemoveAddress(clientId, addressId)).Throws(new Exception(exceptionMessage));

            var controller = new AddressController(AddressService.Object, Logger.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
            controller.ControllerContext.HttpContext.Items.Add("clientId", clientId);

            var actionResult = controller.Delete(addressId);
            var result = actionResult as OkObjectResult;
            var result1 = actionResult as ObjectResult;

            AddressService.Verify(x => x.RemoveAddress(clientId, addressId), Times.Once);
            Assert.IsTrue(result == null);
            Assert.IsTrue(result1 != null);
            Assert.AreEqual(result1.StatusCode, 500);
            Assert.AreEqual(result1.Value, exceptionMessage);
        }
    }
}
