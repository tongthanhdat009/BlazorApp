// File: BlazorApp/Dto/CartItemDto.cs
using System;

namespace BlazorApp.Dto
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }      // ID từ backend
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime AddedAt { get; set; }    // đồng bộ với backend (không nullable)

        public ProductDto? Product { get; set; } // navigation object

        // Helper properties
        public string? ProductName => Product?.ProductName;
        public string? ImageUrl => Product?.ImageUrl;
    }
}

