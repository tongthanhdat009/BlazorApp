using BlazorApp.Dto;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> CreateOrderFromCartAsync(CreateOrderFromCartRequest request);
        Task<OrderDto?> PreviewOrderFromCartAsync(CreateOrderFromCartRequest request);
        Task<OrderDto?> CheckoutFromCartAsync(CreateOrderFromCartRequest request);
        Task<List<OrderDto>> GetMyOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
    }

    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;

        public OrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<OrderDto?> CreateOrderFromCartAsync(CreateOrderFromCartRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/customer/orders/create-from-cart", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
                return result?.data;
            }
            else
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(err?.message ?? "Lỗi khi tạo đơn hàng");
            }
        }

        public async Task<OrderDto?> PreviewOrderFromCartAsync(CreateOrderFromCartRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/customer/orders/preview", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
                return result?.data;
            }
            else
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(err?.message ?? "Lỗi khi xem trước đơn hàng");
            }
        }

        public async Task<OrderDto?> CheckoutFromCartAsync(CreateOrderFromCartRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/customer/orders/checkout", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
                return result?.data;
            }
            else
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(err?.message ?? "Lỗi khi thanh toán đơn hàng");
            }
        }

        public async Task<List<OrderDto>> GetMyOrdersAsync()
        {
            var orders = await _http.GetFromJsonAsync<List<OrderDto>>("api/customer/orders");
            return orders ?? new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            return await _http.GetFromJsonAsync<OrderDto>($"api/customer/orders/{orderId}");
        }

        // Helper DTO parse response
        private class ApiResponse<T>
        {
            public string message { get; set; } = "";
            public T data { get; set; } = default!;
        }

        private class ErrorResponse
        {
            public string message { get; set; } = "";
        }
    }
}
