namespace BlazorApp.Dto;

public class PromotionDto
    {
        public int PromoId { get; set; }
        public string PromoCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = string.Empty; // "percentage" hoặc "fixed"
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }   // đổi từ DateOnly -> DateTime
        public DateTime EndDate { get; set; }     // đổi từ DateOnly -> DateTime
        public decimal? MinOrderAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int? UsedCount { get; set; }
        public string? Status { get; set; }
    }
