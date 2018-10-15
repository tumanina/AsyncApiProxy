using AsyncApiProxy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiProxy.Controllers
{
    [Route("api/messages")]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public void Post([FromBody]string message)
        {
            _messageService.SendMessage(message);
        }
    }
}
