using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet("{ps}/{page}/{q}")]
        public async Task<IActionResult> Get([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string? q = null)
        {
            _logger.LogInformation("running get");

            Expression<Func<Product, bool>> expression = x => x.Active;
            var products = await _productRepository.GetAll(ps, page, expression);

            return Ok(products);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            _logger.LogInformation("running delete");

            await _productRepository.Remove(id);
            var result = await _productRepository.UnitOfWork.Commit();

            return result ? NoContent() : BadRequest();
        }
    }
}
