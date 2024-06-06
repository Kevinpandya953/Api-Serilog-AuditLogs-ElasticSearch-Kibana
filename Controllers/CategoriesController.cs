using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.UOW;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories(int pageNumber = 1, int pageSize = 5)
        {
            var categoriesRepository = _unitOfWork.GetRepository<Categories>();
            var categories = await categoriesRepository.GetAllPaged(pageNumber, pageSize);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategoriesById(int id)
        {
            var categoriesRepository = _unitOfWork.GetRepository<Categories>();
            var categories = await categoriesRepository.GetByIdAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> PostCategories(Categories categories)
        {
            var categoriesRepository = _unitOfWork.GetRepository<Categories>();
            await categoriesRepository.AddAsync(categories);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = categories.category_id }, categories);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCategories(int id)
        {
            var categoriesRepository = _unitOfWork.GetRepository<Categories>();
            var existingCategories = await categoriesRepository.GetByIdAsync(id);
            if (existingCategories == null)
            {
                return NotFound();
            }
            await categoriesRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
