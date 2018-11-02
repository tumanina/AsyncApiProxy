using System;

namespace AsyncApiProxy.Api.Areas.V1.Models
{
    public class Task
    {
        public Task(BusinessLogic.Models.Task model)
        {
            Id = model.Id;
            Type = model.Type;
            Data = model.Data;
            Result = model.Result;
            Status = model.Status;
            Error = model.Error;
        }

        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }

    }
}
