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

    }
}
