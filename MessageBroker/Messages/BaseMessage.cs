using Newtonsoft.Json;

namespace MessageBroker.Messages
{
    public class BaseMessage
    {
        [JsonProperty("task_id")]
        public long TaskId { get; set; }

        [JsonProperty("callback_queue_name")]
        public string CallbackQueueName { get; set; }
    }
}
