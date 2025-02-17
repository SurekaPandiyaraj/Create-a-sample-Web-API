using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using OrderService.Models;
using OrderService.Database; // Ensure you have this installed via NuGet

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _ordercontext;
        private readonly HttpClient _httpClient;

        public OrderController(OrderDbContext ordercontext, HttpClient httpClient)
        {
            _ordercontext = ordercontext;
            _httpClient = httpClient;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordercontext.Orders.ToListAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            return Ok(orders);
            var productIds = orders.Select(o => o.productId).Distinct().ToList();
            var products = new List<object>();


            foreach (var productId in productIds)
            {
                var response = await _httpClient.GetAsync($"https://localhost:7011/api/Product/{productId}");

                if (response.IsSuccessStatusCode)
                {

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<object>(jsonString);
                    products.Add(product);
                }
            }

            var ordersWithProductDetails = orders.Select(order => new
            {
                order.Id,
                order.productId,
                order.Quantity,
                order.CustomerName,
                order.Address,
                Product = products.FirstOrDefault(p =>
                    (int)p.GetType().GetProperty("Id")?.GetValue(p, null) == order.productId)
            });

            return Ok(ordersWithProductDetails);
        }

        // GET: api/Order/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetOrdersByProductId(int productId)
        {
            var orders = await _ordercontext.Orders
                .Where(o => o.productId == productId)
                .ToListAsync();

            if (!orders.Any())
            {
                return NotFound($"No orders found for product ID {productId}.");
            }

            var response = await _httpClient.GetAsync($"https://localhost:7011/api/Product/{productId}");
            object product = null;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<object>(jsonString);
            }

            var ordersWithProductDetails = orders.Select(order => new
            {
                order.Id,
                order.productId,
                order.Quantity,
                order.CustomerName,
                order.Address,
                Product = product
            });

            return Ok(ordersWithProductDetails);
        }

        // ...

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            if (order == null || order.Quantity <= 0)
            {
                return BadRequest(new { message = "Invalid order data." });
            }

            // Check if the product exists in ProductService
            var response = await _httpClient.GetAsync($"https://localhost:7011/api/Product/{order.productId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound(new { message = "Product not found." });
            }

            // Convert response to object
            var product = await response.Content.ReadFromJsonAsync<object>();

            // Save order in database
            _ordercontext.Orders.Add(order);
            await _ordercontext.SaveChangesAsync();

            // Return the order with product details
            var orderWithProduct = new
            {
                order.Id,
                order.productId,
                order.Quantity,
                order.CustomerName,
                order.Address,
                Product = product
            };

            return CreatedAtAction(nameof(GetOrdersByProductId), new { productId = order.productId }, orderWithProduct);
        }

    }
}
