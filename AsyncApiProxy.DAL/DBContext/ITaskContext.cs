using AsyncApiProxy.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AsyncApiProxy.DAL.DBContext
{
    public interface ITaskContext: IDisposable
    {
        DbSet<Task> Task { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
