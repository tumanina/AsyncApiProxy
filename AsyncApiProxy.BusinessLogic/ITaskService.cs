using AsyncApiProxy.BusinessLogic.Models;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public interface ITaskService
    {
        Task GetTask(Guid id);
    }
}
