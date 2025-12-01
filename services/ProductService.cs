using System.Net.Http.Json;
using BlazorApp.Dto;

namespace BlazorApp.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<List<ProductDto>> SearchProductsAsync(string keyword);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public ProductService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/customer/products");
                return products ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
                return new List<ProductDto>();
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProductDto>($"api/customer/products/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading product: {ex.Message}");
                return null;
            }
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/customer/products/categories");
                return categories ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
                return new List<CategoryDto>();
            }
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string keyword)
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>($"api/customer/products/search?keyword={keyword}");
                return products ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching products: {ex.Message}");
                return new List<ProductDto>();
            }
        }
    }
}
