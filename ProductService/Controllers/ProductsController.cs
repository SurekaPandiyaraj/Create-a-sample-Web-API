using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    using System.Net.Http;
    using Common.Contracts;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProductService.Database;
    using ProductService.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductController(ProductDbContext context, HttpClient httpClient, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _httpClient = httpClient;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync();

            if (result == 1)
            {
                return Ok(product);
            }
            return Ok("error");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }




        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _publishEndpoint.Publish<IProductCreated>(new
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock

            });

            return Ok("Product Created and Event Published!");
        }
    }

}
