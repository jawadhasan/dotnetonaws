using demoApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsRepository _productsRepository;

        public ProductsController(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = _productsRepository.GetAll();
                return Ok(products);
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
}
