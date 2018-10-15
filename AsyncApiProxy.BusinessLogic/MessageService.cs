using MessageBroker;
using MessageBroker.Messages;
using Newtonsoft.Json;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public class MessageService : IMessageService
    {
        private readonly IRequestManager _requestManager;

        public MessageService(IRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public Result SendMessage(string text)
        {
            var taskId = Guid.NewGuid();

            var callbackQueueName = $"{taskId}_CallbackQueue";

            var message = new TextMessage
            {
                Text = text,
                CallbackQueueName = callbackQueueName,
                TaskId = taskId,
            };

            var result = _requestManager.TryToExecute(MessageType.Test.ToString(), JsonConvert.SerializeObject(message), callbackQueueName);

            if (result.Result)
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<Result>(result.Value);

                    if (!string.IsNullOrEmpty(response?.Value))
                    {
                        return response;
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Problem with response deserialization.");
                }
            }

            return new Result { TaskId = taskId };
        }
    }
}
