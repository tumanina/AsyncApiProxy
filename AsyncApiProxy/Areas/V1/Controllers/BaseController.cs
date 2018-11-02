using Microsoft.AspNetCore.Mvc;
using System;

namespace AsyncApiProxy.Api.Areas.V1.Controllers
{
    public class BaseController: Controller
    {
        protected ObjectResult InternalServerError(Exception ex)
        {
            var message = ex.InnerMessage();
            return StatusCode(500, message);
        }

        protected ObjectResult InternalServerError(string message)
        {
            return StatusCode(500, message);
        }

        protected ObjectResult Unauthorized(string message)
        {
            return StatusCode(401, message);
        }

        protected string GetCreatedUrl(string id)
        {
            return Request.Scheme + "://" + Request.Host + Request.Path + "/" + id;
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
