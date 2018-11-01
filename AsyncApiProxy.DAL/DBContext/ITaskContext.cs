using AsyncApiProxy.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsyncApiProxy.DAL.DBContext
{
    public interface ITaskContext
    {
        DbSet<Task> Task { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
