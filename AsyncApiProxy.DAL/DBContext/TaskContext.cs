﻿using AsyncApiProxy.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsyncApiProxy.DAL.DBContext
{
    public class TaskContext : DbContext, ITaskContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
        {
        }

        public DbSet<Task> Task { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Task>(b =>
            {
                b.HasKey(u => u.Id);
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.Type).HasColumnName("Type");
                b.Property(t => t.Data).HasColumnName("Data");
                b.Property(t => t.Result).HasColumnName("Result");
                b.Property(t => t.Status).HasColumnName("Status");
                b.Property(t => t.Error).HasColumnName("Error");
                b.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
                b.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
                b.ToTable("task");
            });
        }
    }
}