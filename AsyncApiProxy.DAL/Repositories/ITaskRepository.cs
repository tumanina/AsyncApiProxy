using AsyncApiProxy.DAL.Entities;
using System;

namespace AsyncApiProxy.DAL.Repositories
{
    public interface ITaskRepository
    {
        Task GetTask(Guid id);
        Task CreateTask(int type, int status, string data);
        Task UpdateStatus(Guid id, int status);
    }
}
