namespace BlazorApp.Dto
{
     public class OrderDto
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? PromoId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? PayStatus { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? OrderType { get; set; }
        public string? PaymentMethod { get; set; }
        
        // Thông tin giao hàng
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        
        public List<OrderItemWithProductDto> OrderItems { get; set; } = new();
    }

     public class OrderItemDto
    {
        public int OrderItemId { get; set; }     
        public int OrderId { get; set; }         
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public ProductDto? Product { get; set; } // navigation object

        // helper property để Razor dễ dùng
        public string? ProductName => Product?.ProductName;
    }


    public class CreateOrderFromCartRequest
    {
        public int? PromoId { get; set; }
        // Frontend may send either PromoCode (string) or PromoId (int). Backend Checkout endpoint expects PromoCode.
        public string? PromoCode { get; set; }
        // Optional: frontend-calculated discount amount (backend should always verify/recompute)
        public decimal? DiscountAmount { get; set; }
        public string PaymentMethod { get; set; } = "cash"; // cash hoặc card
        
        // Customer information
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
    }


    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "cash";
        public DateTime PaymentDate { get; set; }
    }

}
