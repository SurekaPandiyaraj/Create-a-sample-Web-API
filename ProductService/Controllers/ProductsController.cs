using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(new List<string> { "Product1", "Product2" });
        }
    }
}
