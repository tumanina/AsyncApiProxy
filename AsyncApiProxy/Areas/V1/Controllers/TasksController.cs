using System;
using AsyncApiProxy.Api.Areas.V1.Models;
using AsyncApiProxy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiProxy.Api.Areas.V1.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Methods for viewing and managing tasks.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class TasksController : BaseController
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Initialization.
        /// </summary>
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Returns task details by Id.
        /// </summary>
        /// <param name="id">Task identifier</param>
        /// <returns>Execution status (ОК/500) and task details/error information.</returns>
        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var task = _taskService.GetTask(id);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(new Task(task));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
