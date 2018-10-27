using Newtonsoft.Json;
using System;

namespace AsyncApiProxy.BusinessLogic.Models
{
    public class Result
    {
        [JsonProperty("TaskId")]
        public Guid? TaskId { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_code")]
        public int? ErrorCode { get; set; }
    }
}
