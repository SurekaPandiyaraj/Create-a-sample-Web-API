using Microsoft.EntityFrameworkCore;
using OrderService.Controllers;
using OrderService.Models;
using System.Collections.Generic;


namespace OrderService.Database
{
    public class OrderDbContext: DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        // Define DbSets for your entities here
        public DbSet<Order> Orders { get; set; }
        public DbSet<Orderdata> OrderData { get; set; }
    }
}
