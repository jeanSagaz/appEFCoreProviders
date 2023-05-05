using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public CustomersController(ILogger<CustomersController> logger,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string? q = null)
        {
            _logger.LogInformation("running get");

            Expression<Func<Customer, bool>> expression = x => x.DateUpdated == null;
            var customers = await _customerRepository.GetAll(ps, page, expression);
            
            var products = await _productRepository.GetAll(ps, page);

            return Ok(customers);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            _logger.LogInformation("running delete");

            await _customerRepository.Remove(id);
            var result = await _customerRepository.UnitOfWork.Commit();

            return result ? NoContent() : BadRequest();
        }
    }
}