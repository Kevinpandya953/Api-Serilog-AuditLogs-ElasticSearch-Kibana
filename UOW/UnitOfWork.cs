using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Repository;
using WebApplication1.Data;

namespace WebApplication1.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BikeStoresDbContext _context;
        public UnitOfWork(BikeStoresDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
