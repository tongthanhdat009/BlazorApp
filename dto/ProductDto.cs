namespace BlazorApp.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public string? Unit { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? Quantity { get; set; }
        
        // Nested objects from API
        public CategoryDto? Category { get; set; }
        public SupplierDto? Supplier { get; set; }
        
        // Helper properties for backward compatibility
        public string? CategoryName => Category?.CategoryName;
        public string? SupplierName => Supplier?.Name;
        public bool IsInStock => Quantity.HasValue && Quantity.Value > 0;
        public string StockStatus => IsInStock ? "Còn hàng" : "Hết hàng";
        
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
