namespace BlazorApp.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? PromoId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public string OrderType { get; set; }
        public List<OrderItemWithProductDto> OrderItems { get; set; }
    }

    
}
