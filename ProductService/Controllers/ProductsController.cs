using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly HttpClient _httpClient;

        //public ProductsController(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(new List<string> { "Product1", "Product2" });
        }
        [HttpPost]
        public IActionResult CreateProduct([FromForm] string product)
        {
            return Ok(new { Message = "Product Created", Product = product });
        }

        //[HttpPost("{productId}/create-order")]
        //public async Task<IActionResult> CreateOrder(int productId,  OrderRequest orderRequest)
        //{
        //    if (orderRequest == null || string.IsNullOrEmpty(orderRequest.CustomerName))
        //    {
        //        return BadRequest("Invalid order request.");
        //    }

        //    var order = new
        //    {
        //        OrderId = new Random().Next(1000, 9999),
        //        ProductId = productId,
        //        CustomerName = orderRequest.CustomerName
        //    };

        //    var content = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync("http://localhost:5068/api/Order", content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return StatusCode((int)response.StatusCode, "Failed to create order.");
        //    }

        //    var createdOrder = await response.Content.ReadAsStringAsync();
        //    return Ok(new { Message = "Order Created", Order = createdOrder });
        //}
    }

    public class OrderRequest
    {
        public string CustomerName { get; set; }
    }
}

