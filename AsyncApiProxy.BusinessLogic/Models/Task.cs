using System;

namespace AsyncApiProxy.BusinessLogic.Models
{
    public class Task
    {
        public Task()
        {
        }

        public Task(DAL.Entities.Task entity)
        {
            Id = entity.Id;
            Type = (TaskType)entity.Type;
            Data = entity.Data;
            Result = entity.Result;
            Status = (TaskStatus)entity.Status;
            Error = entity.Error;
        }

        public Guid Id { get; set; }
        public TaskType Type { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
        public TaskStatus Status { get; set; }
        public string Error { get; set; }

    }
}
