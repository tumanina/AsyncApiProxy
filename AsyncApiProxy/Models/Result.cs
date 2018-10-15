using System;

namespace AsyncApiProxy.Api.Models
{
    public class Result
    {
        public string Value { get; set; }
        public Guid? TaskId { get; set; }
    }
}
