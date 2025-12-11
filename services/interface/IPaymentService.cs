using BlazorApp.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto?> CreatePaymentAsync(int orderId, PaymentDto paymentDto);
        Task<List<PaymentDto>> GetPaymentsByOrderAsync(int orderId);
    }
}
