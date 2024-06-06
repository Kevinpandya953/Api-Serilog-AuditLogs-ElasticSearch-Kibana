using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.UOW;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brands>>> GetBrands(int pageNumber = 1, int pageSize = 5)
        {
            var brandsRepository = _unitOfWork.GetRepository<Brands>();
            var brands = await brandsRepository.GetAllPaged(pageNumber, pageSize);
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brands>> GetBrands(int id)
        {

            var brandsRepository = _unitOfWork.GetRepository<Brands>();
            var brands = await brandsRepository.GetByIdAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpPost]
        public async Task<ActionResult> PostBrands(Brands brands)
        {
            var brandsRepository = _unitOfWork.GetRepository<Brands>();
            await brandsRepository.AddAsync(brands);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetBrands), new { id = brands.brand_id }, brands);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBrands(int id)
        {
            var brandsRepository = _unitOfWork.GetRepository<Brands>();
            var existingBrand = await brandsRepository.GetByIdAsync(id);
            if (existingBrand == null)
            {
                return NotFound();

            }

            await brandsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
