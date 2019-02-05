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
        private readonly ITaskService _taskService;

        public ClientService(ITaskService taskService, IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _taskService = taskService;
        }

        public CreateClientResult CreateClient(Client client)
        {
            var task = _taskService.CreateTask(TaskType.CreateClient, JsonConvert.SerializeObject(new { client.Email, client.Name, client.Password }));

            var callbackQueueName = $"{task.Id}_CallbackQueue";

            var message = new CreateClientMessage
            {
                Name = client.Name,
                Email = client.Email,
                CallbackQueueName = callbackQueueName,
                TaskId = task.Id,
            };

            var result = _requestManager.TryToExecute(MessageType.CreateClient.ToString(), JsonConvert.SerializeObject(message), callbackQueueName);
            if (result.Result)
            {
                try
                {
                    return JsonConvert.DeserializeObject<CreateClientResult>(result.Value);
                }
                catch (Exception)
                {
                    throw new Exception($"Problem with response deserialization: {result.Value}.");
                }
            }

            return new CreateClientResult { TaskId = task.Id };
        }
    }
}
