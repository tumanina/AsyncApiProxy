using Newtonsoft.Json;
using System;

namespace MessageBroker.Messages
{
    public class BaseMessage
    {
        [JsonProperty("task_id")]
        public Guid TaskId { get; set; }

        [JsonProperty("callback_queue_name")]
        public string CallbackQueueName { get; set; }
    }
}
