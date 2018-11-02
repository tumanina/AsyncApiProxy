using AsyncApiProxy.Api.Areas.V1.Models;
using AsyncApiProxy.Api.Models;
using AsyncApiProxy.BusinessLogic;
using AsyncApiProxy.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AsyncApiProxy.Api.Areas.V1.Controllers
{
    [Route("api/v1/[controller]")]
    public class ClientsController : BaseController
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateClientRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request is empty or has invalid format");
                }
                if (string.IsNullOrEmpty(request.Name) || request.Name.Length > 32)
                {
                    return BadRequest("Client name is empty or has length more than 32.");
                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest("Email is empty.");
                }

                var result = _clientService.CreateClient(new Client { Name = request.Name, Email = request.Email } );

                if (result.TaskId.HasValue)
                {
                    return Ok(new TimeOutResult { TaskId = result.TaskId.Value });
                }

                if (result.ErrorCode.HasValue)
                {
                    return InternalServerError(result.Error);
                }

                return Ok(new Models.CreateClientResult { Id = result.Id });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
