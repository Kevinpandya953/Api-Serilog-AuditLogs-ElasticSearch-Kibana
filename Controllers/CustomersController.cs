using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Repository;
using WebApplication1.Models;
using WebApplication1.UOW;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BikeStoresDbContext _context;
        public CustomersController(IUnitOfWork unitOfWork, BikeStoresDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers(int pageNumber = 1, int pageSize = 5)
        {
            var customersRepository = _unitOfWork.GetRepository<Customers>();
            var customers = await customersRepository.GetAllPaged(pageNumber, pageSize);
            return Ok(customers);

            
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>>GetCustomers(int id)
        {
            var customersRepository = _unitOfWork.GetRepository<Customers>();
            var customers = await customersRepository.GetByIdAsync(id);
            if(customers  == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<Customers>>PostCustomers(Customers customers)
        {
            var customersRepository = _unitOfWork.GetRepository<Customers>();
            await customersRepository.AddAsync(customers);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetCustomers), new { id = customers.customer_id }, customers);

        }
        [HttpDelete]
        public async Task<ActionResult<Customers>> DeleteCustomers(int id)
        {
            var customersRepository = _unitOfWork.GetRepository<Customers>();
            var existingCustomers = await customersRepository.GetByIdAsync(id);
            if(existingCustomers == null)
            {
                return NotFound();
            }

            await customersRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("GetByCityName")]
        public async Task<IActionResult>GetByCityName(string City)
        {
            var CityName = await _context.Customers
                .Where(c => c.city == City).ToListAsync();
            return Ok(CityName);
        }

        [HttpGet("GetNotNULL")]

        public async Task<IActionResult>GetByNotNull()
        {
            var phonedetails = await _context.Customers
                .Where(c=> c.phone != null).ToListAsync();
            return Ok(phonedetails);    
        }
        [HttpGet("FilterByCityAndState")]
        public async Task<IActionResult> FilterByCityAndState(string City, string State)
        {
            var details = await _context.Customers
                .Where(c=> c.city==City && c.state==State).ToListAsync();
            return Ok(details);
        }
    }
}
