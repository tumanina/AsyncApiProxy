using AsyncApiProxy.DAL.DBContext;
using AsyncApiProxy.DAL.Entities;
using System;
using System.Linq;

namespace AsyncApiProxy.DAL.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly ITaskContextFactory _factory;

        public TaskRepository(ITaskContextFactory factory)
        {
            _factory = factory;
        }

        public Task GetTask(Guid id)
        {
            using (var context = _factory.CreateDBContext())
            {
                return context.Task.FirstOrDefault(t => t.Id == id);
            }
        }

        public Task CreateTask(int type, int status, string data)
        {
            using (var context = _factory.CreateDBContext())
            {
                var tasks = context.Set<Task>();

                var task = new Task
                {
                    Type = type,
                    Data = data,
                    Status = status,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                tasks.Add(task);

                context.SaveChanges();

                return GetTask(task.Id);
            }
        }

        public Task UpdateStatus(Guid id, int status)
        {
            using (var context = _factory.CreateDBContext())
            {
                var result = context.Task.SingleOrDefault(b => b.Id == id);
                if (result != null)
                {
                    result.Status = status;
                    context.SaveChanges();
                }
                else
                {
                    return null;
                }

                return GetTask(id);
            }
        }
    }
}
