using BlazorApp.Dto;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public interface IPromotionService
    {
        Task<ApplyPromoResultDto?> ApplyPromoCodeAsync(string promoCode, decimal totalAmount);
    }

    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _http;

        public PromotionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ApplyPromoResultDto?> ApplyPromoCodeAsync(string promoCode, decimal totalAmount)
        {
            var payload = new { PromoCode = promoCode, TotalAmount = totalAmount };
            var response = await _http.PostAsJsonAsync("api/promotion/apply", payload);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApplyPromoResultDto>();
            }
            else
            {
                return null;
            }
        }
    }

    public class ApplyPromoResultDto
    {
        public int PromoId { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
