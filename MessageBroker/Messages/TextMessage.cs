using Newtonsoft.Json;

namespace MessageBroker.Messages
{
    public class TextMessage: BaseMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
