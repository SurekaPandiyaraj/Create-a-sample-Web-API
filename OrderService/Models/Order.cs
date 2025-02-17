namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }
}
