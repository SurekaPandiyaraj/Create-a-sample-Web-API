﻿namespace Common.Contracts
{
    public interface IProductCreated
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; } 

    }
}
