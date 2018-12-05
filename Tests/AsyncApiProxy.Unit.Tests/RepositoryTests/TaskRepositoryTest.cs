using AsyncApiProxy.DAL.Entities;
using AsyncApiProxy.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System;
using AsyncApiProxy.DAL.Repositories;

namespace AsyncApiProxy.Unit.Tests.RepositoryTests
{
    [TestClass]
    public class TaskRepositoryTest
    {
        private static readonly Mock<ITaskContext> TaskContext = new Mock<ITaskContext>();
        private static readonly Mock<ITaskContextFactory> TaskContextFactory = new Mock<ITaskContextFactory>();

        [TestMethod]
        public void GetTask_TaskExisted_UseDbContextReturnCorrect()
        {
            TaskContext.Invocations.Clear();

            var mockSet = new Mock<DbSet<Task>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var typeId1 = 1;
            var typeId2 = 2;
            var typeId3 = 3;
            var statusId1 = 1;
            var statusId2 = 2;
            var statusId3 = 5;
            var data1 = "123455";
            var data2 = "123456";
            var data3 = "123457";

            var data = new List<Task>
            {
                new Task { Id = id1, Type = typeId1, Status = statusId1, Data = data1 },
                new Task { Id = id2, Type = typeId2, Status = statusId2, Data = data2 },
                new Task { Id = id3, Type = typeId3, Status = statusId3, Data = data3 }
            }.AsQueryable();

            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            TaskContext.Setup(x => x.Task).Returns(mockSet.Object);
            TaskContextFactory.Setup(x => x.CreateDBContext()).Returns(TaskContext.Object);

            var repository = new TaskRepository(TaskContextFactory.Object);

            var result = repository.GetTask(id2);

            TaskContext.Verify(x => x.Task, Times.Once);
            Assert.AreEqual(result.Type, typeId2);
            Assert.AreEqual(result.Status, statusId2);
            Assert.AreEqual(result.Data, data2);
            Assert.AreEqual(result.Id, id2);
        }

        [TestMethod]
        public void GetTask_TaskNotExisted_UseDbContextReturnNull()
        {
            TaskContext.Invocations.Clear();
            var mockSet = new Mock<DbSet<Task>>();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var typeId1 = 1;
            var typeId3 = 3;
            var statusId1 = 1;
            var statusId3 = 5;
            var data1 = "123455";
            var data3 = "123457";

            var data = new List<Task>
            {
                new Task { Id = id1, Type = typeId1, Status = statusId1, Data = data1 },
                new Task { Id = id3, Type = typeId3, Status = statusId3, Data = data3 }
            }.AsQueryable();

            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            TaskContext.Setup(x => x.Task).Returns(mockSet.Object);
            TaskContextFactory.Setup(x => x.CreateDBContext()).Returns(TaskContext.Object);

            var repository = new TaskRepository(TaskContextFactory.Object);

            var result = repository.GetTask(id2);

            TaskContext.Verify(x => x.Task, Times.Once);
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CreateTask_Correct_UseDbContextReturnCorrect()
        {
            TaskContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var typeId1 = 1;
            var statusId1 = 1;
            var data1 = "123455";

            var task = new Task();

            var mockSet0 = new Mock<DbSet<Task>>();

            var data0 = new List<Task>().AsQueryable();

            mockSet0.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data0.Provider);
            mockSet0.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data0.Expression);
            mockSet0.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data0.ElementType);
            mockSet0.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data0.GetEnumerator());
            mockSet0.Setup(m => m.Add(It.IsAny<Task>()))
                .Callback((Task taskParam) => { task = taskParam; });

            var data = new List<Task>
            {
                new Task { Id = id1, Type = typeId1, Status = statusId1, Data = data1 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Task>>();

            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Task>()));

            TaskContext.Setup(x => x.Task).Returns(mockSet.Object);
            TaskContext.Setup(x => x.Set<Task>()).Returns(mockSet0.Object);
            TaskContext.Setup(x => x.SaveChanges()).Returns(1);
            TaskContextFactory.Setup(x => x.CreateDBContext()).Returns(TaskContext.Object);

            var repository = new TaskRepository(TaskContextFactory.Object);

            var result = repository.CreateTask(typeId1, statusId1, data1);

            TaskContext.Verify(x => x.Set<Task>(), Times.Once);
            TaskContext.Verify(x => x.SaveChanges(), Times.Once);
            mockSet0.Verify(x => x.Add(It.IsAny<Task>()), Times.Once);
            Assert.AreEqual(task.Type, typeId1);
            Assert.AreEqual(task.Status, statusId1);
            Assert.AreEqual(task.Data, data1);
        }

        [TestMethod]
        public void UpdateTaskStatus_TaskExist_UseDbContextReturnCorrect()
        {
            TaskContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var typeId1 = 1;
            var statusId1 = 1;
            var statusId2 = 2;
            var data1 = "123455";

            var data = new List<Task>
            {
                new Task { Id = id1, Type = typeId1, Status = statusId1, Data = data1 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Task>>();

            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Task>()));

            TaskContext.Setup(x => x.Task).Returns(mockSet.Object);
            TaskContext.Setup(x => x.Set<Task>()).Returns(mockSet.Object);
            TaskContext.Setup(x => x.SaveChanges()).Returns(1);
            TaskContextFactory.Setup(x => x.CreateDBContext()).Returns(TaskContext.Object);

            var repository = new TaskRepository(TaskContextFactory.Object);

            var result = repository.UpdateStatus(id1, statusId2);

            Assert.AreEqual(result.Status, statusId2);
            Assert.AreEqual(result.Id, id1);
            TaskContext.Verify(x => x.Task, Times.Exactly(2));
            TaskContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateTaskStatus_TaskNotExisted_UseDbContextReturnCorrect()
        {
            TaskContext.Invocations.Clear();

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var typeId1 = 1;
            var statusId1 = 1;
            var statusId2 = 2;
            var data1 = "123455";

            var data = new List<Task>
            {
                new Task { Id = id1, Type = typeId1, Status = statusId1, Data = data1 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Task>>();

            mockSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Task>()));

            TaskContext.Setup(x => x.Task).Returns(mockSet.Object);
            TaskContext.Setup(x => x.Set<Task>()).Returns(mockSet.Object);
            TaskContext.Setup(x => x.SaveChanges()).Returns(1);
            TaskContextFactory.Setup(x => x.CreateDBContext()).Returns(TaskContext.Object);

            var repository = new TaskRepository(TaskContextFactory.Object);

            var result = repository.UpdateStatus(id2, statusId2);

            TaskContext.Verify(x => x.Task, Times.Exactly(1));
            TaskContext.Verify(x => x.SaveChanges(), Times.Never);
            Assert.AreEqual(result, null);
        }
    }
}
