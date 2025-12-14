namespace BlazorApp.Dto
{
    public class CancelOrderDto
    {
        public string? Reason { get; set; }
        public string? CustomerBankName { get; set; }
        public string? CustomerBankAccount { get; set; }
        public string? CustomerAccountHolder { get; set; }
    }
}
