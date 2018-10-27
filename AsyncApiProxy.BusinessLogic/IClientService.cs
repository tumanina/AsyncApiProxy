using AsyncApiProxy.BusinessLogic.Models;

namespace AsyncApiProxy.BusinessLogic
{
    public interface IClientService
    {
        CreateClientResult CreateClient(Client client);
    }
}
