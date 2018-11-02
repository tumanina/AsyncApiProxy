namespace AsyncApiProxy.DAL.DBContext
{
    public interface ITaskContextFactory
    {
        ITaskContext CreateDBContext();
    }
}
