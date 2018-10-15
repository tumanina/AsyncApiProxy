using Microsoft.AspNetCore.Mvc;
using System;

namespace AsyncApiProxy.Api.Controllers
{
    public class BaseController: Controller
    {
        protected ObjectResult InternalServerError(Exception ex)
        {
            return StatusCode(500, ex.InnerMessage());
        }

        private ObjectResult StatusCode(int v, object p)
        {
            throw new NotImplementedException();
        }

        protected ObjectResult InternalServerError(string message)
        {
            return StatusCode(500, message);
        }

        protected ObjectResult Unauthorized(string message)
        {
            return StatusCode(401, message);
        }
    }

    public static class Extensions
    {
        public static string InnerMessage(this Exception exception)
        {
            while (true)
            {
                if (exception.InnerException == null) return exception.Message;
                exception = exception.InnerException;
            }
        }
    }
}
