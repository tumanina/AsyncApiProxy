using AsyncApiProxy.BusinessLogic.Models;
using AsyncApiProxy.DAL.Repositories;
using MessageBroker;
using MessageBroker.Messages;
using Newtonsoft.Json;
using System;

namespace AsyncApiProxy.BusinessLogic
{
    public class ClientService : IClientService
    {
        private readonly IRequestManager _requestManager;
        private readonly ITaskRepository _taskRepository;

        public ClientService(ITaskRepository taskRepository, IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _taskRepository = taskRepository;
        }

        public CreateClientResult CreateClient(Client client)
        {
            var task = _taskRepository.CreateTask((int)TaskType.CreateClient, (int)TaskStatus.Created, JsonConvert.SerializeObject(new { client.Email, client.Name }));

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
