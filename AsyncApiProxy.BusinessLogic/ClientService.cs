using AsyncApiProxy.BusinessLogic.Models;
using MessageBroker;
using MessageBroker.Messages;
using Newtonsoft.Json;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public class ClientService : IClientService
    {
        private readonly IRequestManager _requestManager;

        public ClientService(IRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        public CreateClientResult CreateClient(Client client)
        {
            var taskId = Guid.NewGuid();

            var callbackQueueName = $"{taskId}_CallbackQueue";

            var message = new CreateClientMessage
            {
                Name = client.Name,
                Email = client.Email,
                CallbackQueueName = callbackQueueName,
                TaskId = taskId,
            };

            var result = _requestManager.TryToExecute(MessageType.Clients.ToString(), JsonConvert.SerializeObject(message), callbackQueueName);

            if (result.Result)
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<CreateClientResult>(result.Value);

                    return response;
                }
                catch (Exception)
                {
                    throw new Exception("Troubles with response deserialization.");
                }
            }

            return new CreateClientResult { TaskId = taskId };
        }
    }
}
