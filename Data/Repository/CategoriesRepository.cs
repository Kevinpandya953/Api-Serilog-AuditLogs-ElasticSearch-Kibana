using WebApplication1.Models;

namespace WebApplication1.Data.Repository
{
    public class CategoriesRepository: Repository<Categories>
    {
        public CategoriesRepository(BikeStoresDbContext context) : base(context) { }
    }
}
