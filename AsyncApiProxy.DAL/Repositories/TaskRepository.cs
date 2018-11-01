using AsyncApiProxy.DAL.DBContext;
using AsyncApiProxy.DAL.Entities;
using System;
using System.Linq;

namespace AsyncApiProxy.DAL.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly ITaskContext _context;

        public TaskRepository(ITaskContext context)
        {
            _context = context;
        }

        public Task GetTask(Guid id)
        {
            return _context.Task.FirstOrDefault(t => t.Id == id);
        }

        public Task CreateTask(int type, int status, string data)
        {
            var tasks = _context.Set<Task>();

            var task = new Task
            {
                Type = type,
                Data = data,
                Status = status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            tasks.Add(task);

            _context.SaveChanges();

            return GetTask(task.Id);
        }

        public Task UpdateStatus(Guid id, int status)
        {
            var result = _context.Task.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                result.Status = status;
                _context.SaveChanges();
            }
            else
            {
                return null;
            }

            return GetTask(id);
        }
    }
}
