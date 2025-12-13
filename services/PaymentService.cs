using BlazorApp.Dto;
using System.Net.Http.Json;

namespace BlazorApp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _http;

        public PaymentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PaymentDto?> CreatePaymentAsync(int orderId, PaymentDto paymentDto)
        {
            var response = await _http.PostAsJsonAsync($"api/customer/orders/{orderId}/pay", paymentDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PaymentDto>>();
                return result?.data;
            }
            else
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(err?.message ?? "Lỗi khi tạo payment");
            }
        }

        public async Task<VNPayResponseDto?> CreateVNPayPaymentAsync(VNPayRequestDto request)
        {
            var response = await _http.PostAsJsonAsync("api/customer/vnpay/create-payment", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<VNPayResponseDto>();
            }
            else
            {
                var err = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new Exception(err?.message ?? "Lỗi khi tạo thanh toán VNPay");
            }
        }

        public async Task<List<PaymentDto>> GetPaymentsByOrderAsync(int orderId)
        {
            // Nếu backend chưa có endpoint lấy payments theo order, bạn cần tạo hoặc bỏ
            // Tạm thời return empty list
            return new List<PaymentDto>();
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
