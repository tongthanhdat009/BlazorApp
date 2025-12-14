using System;

namespace BlazorApp.Dto
{
    public class RefundRequestDto
    {
        public int RefundId { get; set; }
        public int OrderId { get; set; }
        public decimal RefundAmount { get; set; }
        public string? Reason { get; set; }
        public string? CustomerBankName { get; set; }
        public string? CustomerBankAccount { get; set; }
        public string? CustomerAccountHolder { get; set; }
        public string? Status { get; set; }
        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public string? AdminNote { get; set; }
        public string? GatewayRefundId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Thông tin bổ sung từ Order
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
