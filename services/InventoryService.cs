using System.Net.Http.Json;
using BlazorApp.Dto;

namespace BlazorApp.Services
{
    public interface IInventoryService
    {
        Task<ValidateCartStockResponse?> ValidateCartStockAsync(ValidateCartStockRequest request);
    }

    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;

        public InventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ValidateCartStockResponse?> ValidateCartStockAsync(ValidateCartStockRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/inventory/customer/validate-cart-stock", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ValidateCartStockResponse>();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating cart stock: {ex.Message}");
                return null;
            }
        }
    }
}
