using Common.Contracts;
using MassTransit;

namespace OrderService.Consumers
{
    public class ProductCreatedConsumer : IConsumer<IProductCreated>
    {
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IProductCreated> context)
        {
            var product = context.Message;
            _logger.LogInformation($"📩 Received Product Event: ID = {product.ProductId}, Name = {product.ProductName}");

            await Task.Delay(1000);
            _logger.LogInformation("✅ Order Created for Product: " + product.ProductName);
        }
    }
}
