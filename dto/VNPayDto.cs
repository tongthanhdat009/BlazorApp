namespace BlazorApp.Dto
{
    public class VNPayRequestDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; }
    }

    public class VNPayResponseDto
    {
        public bool Success { get; set; }
        public string? PaymentUrl { get; set; }
        public string? Message { get; set; }
    }
}
