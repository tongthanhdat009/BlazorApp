using BlazorApp.Dto;

namespace BlazorApp.Services
{
    public class CartService : ICartService
    {
        private readonly IApiService _api;

        public CartService(IApiService api)
        {
            _api = api;
        }

        // GET api/customer/cart
        public async Task<List<CartItemDto>> GetCartAsync()
        {
            var data = await _api.GetAsync<List<CartItemDto>>("api/customer/cart");
            return data ?? new List<CartItemDto>();
        }

        // POST api/customer/cart/add
        public async Task<bool> AddToCartAsync(int productId, int quantity)
        {
            var req = new { productId, quantity };
            var res = await _api.PostAsync<object, bool>("api/customer/cart/add", req);
            return res;
        }

        // PUT api/customer/cart/update
        public async Task<bool> UpdateQuantityAsync(int productId, int quantity)
        {
            var req = new { productId, quantity };
            var res = await _api.PutAsync<object, bool>("api/customer/cart/update", req);
            return res;
        }

        // DELETE api/customer/cart/{productId}
        public async Task<bool> RemoveItemAsync(int productId)
        {
            return await _api.DeleteAsync($"api/customer/cart/{productId}");
        }

        // DELETE api/customer/cart
        public async Task<bool> ClearCartAsync()
        {
            return await _api.DeleteAsync("api/customer/cart");
        }

        // POST api/customer/cart/validate-checkout
        public async Task<ValidateCheckoutResponse?> ValidateCheckoutAsync(ValidateCheckoutRequest request)
        {
            return await _api.PostAsync<ValidateCheckoutRequest, ValidateCheckoutResponse>(
                "api/customer/cart/validate-checkout", request);
        }
    }
}
