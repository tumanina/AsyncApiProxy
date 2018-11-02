using Microsoft.EntityFrameworkCore;

namespace AsyncApiProxy.DAL.DBContext
{
    public class TaskContextFactory : ITaskContextFactory
    {
        private readonly DbContextOptionsBuilder<TaskContext> _options;

        public TaskContextFactory(DbContextOptionsBuilder<TaskContext> options)
        {
            _options = options;
        }

        public ITaskContext CreateDBContext()
        {
            return new TaskContext(_options.Options);
        }
    }
}
