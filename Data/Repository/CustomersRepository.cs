using WebApplication1.Models;

namespace WebApplication1.Data.Repository
{
    public class CustomersRepository : Repository<Customers>
    {
        public CustomersRepository( BikeStoresDbContext context ) : base( context ) { }
    }
}
