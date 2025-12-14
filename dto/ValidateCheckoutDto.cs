namespace BlazorApp.Dto;

public class ValidateCheckoutRequest
{
    public string? PromoCode { get; set; }
}

public class ValidateCheckoutResponse
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    
    // Inventory validation
    public List<OutOfStockProduct> OutOfStockProducts { get; set; } = new();
    
    // Product deleted validation
    public List<DeletedProduct> DeletedProducts { get; set; } = new();
    
    // Price changed validation
    public List<PriceChangedProduct> PriceChangedProducts { get; set; } = new();
    
    // Promotion validation
    public PromotionValidationResult? PromotionValidation { get; set; }
}

public class PriceChangedProduct
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal CartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
}

public class PromotionValidationResult
{
    public bool IsValid { get; set; }
    public string? Message { get; set; }
    public int? PromoId { get; set; }
    public decimal DiscountAmount { get; set; }
}
