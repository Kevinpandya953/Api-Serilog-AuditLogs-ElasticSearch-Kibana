using WebApplication1.Data.Interfaces;

namespace WebApplication1.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task SaveAsync();
    }
}
