using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.UOW;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            var productsRepository = _unitOfWork.GetRepository<Products>();
            var products = await productsRepository.GetAllAsync();
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts(int id)
        {
            var productsRepository = _unitOfWork.GetRepository<Products>();
            var products = await productsRepository.GetByIdAsync(id);
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost]

        public async Task<ActionResult> PostProducts(Products products)
        {
            var productsRepository = _unitOfWork.GetRepository<Products>();
            await productsRepository.AddAsync(products);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(Products), new { id = products.product_id }, products);

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProducts(int id)
        {
            var productsRepository = _unitOfWork.GetRepository<Products>();
            var existingProducts = await productsRepository.GetByIdAsync(id);
            if(existingProducts == null)
            {
                return NotFound();
            }
            await productsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
