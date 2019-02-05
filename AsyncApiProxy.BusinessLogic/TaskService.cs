using AsyncApiProxy.BusinessLogic.Models;
using AsyncApiProxy.DAL.Repositories;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public class TaskService: ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task CreateTask(TaskType type, string data)
        {
            var task = _taskRepository.CreateTask((int)type, (int)TaskStatus.Created, data);
            return task == null ? null : new Task(task);
        }

        public Task GetTask(Guid id)
        {
            var task = _taskRepository.GetTask(id);
            return task == null ? null : new Task(task);
        }
    }
}
