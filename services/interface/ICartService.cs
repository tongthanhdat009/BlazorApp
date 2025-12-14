using BlazorApp.Dto;

namespace BlazorApp.Services
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartAsync();
        Task<bool> AddToCartAsync(int productId, int quantity);
        Task<bool> UpdateQuantityAsync(int productId, int quantity);
        Task<bool> RemoveItemAsync(int productId);
        Task<bool> ClearCartAsync();
        Task<ValidateCheckoutResponse?> ValidateCheckoutAsync(ValidateCheckoutRequest request);
    }
}
