using WebApplication1.Models;

namespace WebApplication1.Data.Repository
{
    public class ProductsRepository : Repository<Products>
    {
        public ProductsRepository(BikeStoresDbContext context) : base(context) { }
    }
}
