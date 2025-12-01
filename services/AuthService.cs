using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorApp.Dto;
using Microsoft.JSInterop;

namespace BlazorApp.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();
        Task<CustomerInfo?> GetCurrentUserAsync();
        Task<string?> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string TOKEN_KEY = "customer_access_token";
        private const string USER_KEY = "currentUser";

        public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/customer/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    
                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        // Lưu token và user info vào localStorage
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, loginResponse.Token);
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", USER_KEY, 
                            JsonSerializer.Serialize(loginResponse.Customer));

                        // Set default authorization header
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new AuthenticationHeaderValue("Bearer", loginResponse.Token);
                    }

                    return loginResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/customer/auth/register", request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RegisterResponse>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Register failed: {errorContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register error: {ex.Message}");
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            // Xóa token và user info
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", USER_KEY);

            // Remove authorization header
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<CustomerInfo?> GetCurrentUserAsync()
        {
            try
            {
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", USER_KEY);
                
                if (string.IsNullOrEmpty(userJson))
                    return null;

                return JsonSerializer.Deserialize<CustomerInfo>(userJson);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TOKEN_KEY);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task SetAuthorizationHeaderAsync()
        {
            var token = await GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
