using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> orders = new();

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(new List<string> { "Order1", "Order2" });
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order newOrder)
        {
            if (newOrder == null || newOrder.ProductId <= 0)
            {
                return BadRequest("Invalid order details.");
            }

            orders.Add(newOrder);
            return CreatedAtAction(nameof(GetOrdersByProductId), new { productId = newOrder.ProductId }, newOrder);
        }

        [HttpGet("{productId}")]
        public IActionResult GetOrdersByProductId(int productId)
        {
            var productOrders = orders.Where(o => o.ProductId == productId).ToList();
            return Ok(productOrders);
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
    }
}

