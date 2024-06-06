using WebApplication1.Models;

namespace WebApplication1.Data.Repository
{
    public class BrandsRepository : Repository<Brands>
    {
        public BrandsRepository(BikeStoresDbContext context) : base(context) { }
    }
}
