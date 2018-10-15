using AsyncApiProxy.Api.Models;
using AsyncApiProxy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AsyncApiProxy.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]string message)
        {
            try
            {
                var result = _messageService.SendMessage(message);

                if (!string.IsNullOrEmpty(result.Value))
                {
                    return Ok(new Models.Result { Value = result.Value });
                }

                return result.TaskId.HasValue ? Ok(new TimeOutResult { TaskId = result.TaskId.Value }) : StatusCode(500, "Request failed");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
