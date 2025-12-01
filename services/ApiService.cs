using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorApp.Dto;

namespace BlazorApp.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string url);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);
        Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data);
        Task<bool> DeleteAsync(string url);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public ApiService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                return await _httpClient.GetFromJsonAsync<T>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET error: {ex.Message}");
                return default;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                var response = await _httpClient.PostAsJsonAsync(url, data);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TResponse>();
                }
                
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST error: {ex.Message}");
                return default;
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                var response = await _httpClient.PutAsJsonAsync(url, data);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TResponse>();
                }
                
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PUT error: {ex.Message}");
                return default;
            }
        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DELETE error: {ex.Message}");
                return false;
            }
        }
    }
}
