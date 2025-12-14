using System.Net.Http.Json;
using BlazorApp.Dto;
using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazorApp.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOnlineOrdersByCustomerIdAsync();
        Task<List<OrderItemWithProductDto>> GetOrderItemWithProductAsync(int orderId);
        Task<bool> UpdateOrderAndBillStatusAsync(int orderId, string statusOrder, string statusBill);
        Task<byte[]?> DownloadInvoicePdfAsync(int orderId);
        
        Task<OrderDto?> CreateOrderFromCartAsync(CreateOrderFromCartRequest request);
        Task<OrderDto?> PreviewOrderFromCartAsync(CreateOrderFromCartRequest request);
        Task<OrderDto?> CheckoutFromCartAsync(CreateOrderFromCartRequest request);
        Task<List<OrderDto>> GetMyOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<bool> CancelOrderAsync(int orderId, CancelOrderDto cancelDto);
        Task<List<RefundRequestDto>> GetRefundRequestsByOrderIdAsync(int orderId);
    }

    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;
        private readonly IJSRuntime _jsRuntime;

        public OrderService(HttpClient httpClient, IApiService apiService, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _apiService = apiService;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<OrderDto>> GetOnlineOrdersByCustomerIdAsync()
        {
            try
            {
                // Lấy customerId từ localstorage
                int customerId = 0;
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "currentUser");


                if (!string.IsNullOrEmpty(json))
                {
                    var customer = JsonSerializer.Deserialize<CustomerInfo>(json);
                    if (customer != null)
                    {
                        customerId = customer.CustomerId;
                    }
                }

                if (customerId == 0)
                {
                    Console.WriteLine("Không tìm thấy customerId trong localStorage");
                    return new List<OrderDto>();
                }
                var orders = await _apiService.GetAsync<List<OrderDto>>(
                    $"api/customer/orders/online-orders-by-customer/{customerId}"
                );
                return orders ?? new List<OrderDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
                return new List<OrderDto>();
            }
        }

        public async Task<bool> UpdateOrderAndBillStatusAsync(int orderId, string statusOrder, string statusBill)
        {
            var request = new UpdateOrderAndBillStatusDto
            {
                OrderId = orderId,
                StatusOrder = statusOrder,
                StatusBill = statusBill
            };

            var result = await _apiService.PostAsync<UpdateOrderAndBillStatusDto, int>("api/customer/orders/update-order-and-bill-status", request);

            return result > 0;
        }

        public async Task<byte[]?> DownloadInvoicePdfAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/customer/orders/{orderId}/invoice-pdf");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading PDF: {ex.Message}");
                return null;
            }
        }


        public async Task<List<OrderItemWithProductDto>> GetOrderItemWithProductAsync(int orderId)
        {
            try 
            {
                var orderItemWithProducts = await _apiService.GetAsync<List<OrderItemWithProductDto>>(
                    $"api/customer/orders/{orderId}/orderitem-with-product"
                );
                return orderItemWithProducts  ?? new List<OrderItemWithProductDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
                return new List<OrderItemWithProductDto>();
            }
        }

        public async Task<OrderDto?> CreateOrderFromCartAsync(CreateOrderFromCartRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customer/orders/create-from-cart", request);
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
            var response = await _httpClient.PostAsJsonAsync("api/customer/orders/preview", request);
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
            var response = await _httpClient.PostAsJsonAsync("api/customer/orders/checkout", request);
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
            var orders = await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/customer/orders");
            return orders ?? new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            return await _httpClient.GetFromJsonAsync<OrderDto>($"api/customer/orders/{orderId}");
        }

        public async Task<bool> CancelOrderAsync(int orderId, CancelOrderDto cancelDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"api/customer/orders/{orderId}/cancel", cancelDto);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error canceling order: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when canceling order: {ex.Message}");
                return false;
            }
        }

        public async Task<List<RefundRequestDto>> GetRefundRequestsByOrderIdAsync(int orderId)
        {
            try
            {
                var refunds = await _apiService.GetAsync<List<RefundRequestDto>>(
                    $"api/CustomerRefund/order/{orderId}"
                );
                return refunds ?? new List<RefundRequestDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading refund requests: {ex.Message}");
                return new List<RefundRequestDto>();
            }
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
