using Newtonsoft.Json;

namespace MessageBroker.Messages
{
    public class CreateClientMessage : BaseMessage
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
