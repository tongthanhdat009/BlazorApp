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
        Task<bool> UpdateProfileAsync(UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        
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
                    return null;
                }
            }
            catch (Exception)
            {
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
                    return null;
                }
            }
            catch (Exception)
            {
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

        public async Task<bool> UpdateProfileAsync(UpdateProfileRequest request)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync("api/customer/auth/profile", request);

            if (response.IsSuccessStatusCode)
            {
                // Cập nhật thông tin user trong localStorage
                var currentUser = await GetCurrentUserAsync();
                if (currentUser != null)
                {
                    if (!string.IsNullOrEmpty(request.Name))
                        currentUser.Name = request.Name;
                    if (!string.IsNullOrEmpty(request.Phone))
                        currentUser.Phone = request.Phone;
                    if (!string.IsNullOrEmpty(request.Address))
                        currentUser.Address = request.Address;

                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", USER_KEY,
                        JsonSerializer.Serialize(currentUser));
                }
                return true;
            }
            else
            {
                // Đọc error response từ server
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Parse JSON để lấy message
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse>(errorContent, options);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        throw new Exception(errorResponse.Message);
                    }
                }
                catch (JsonException)
                {
                    // Nếu không parse được JSON, throw với content gốc hoặc message mặc định
                }
                
                throw new Exception("Cập nhật thông tin thất bại. Vui lòng thử lại!");
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/customer/auth/change-password", request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // Phân tích lỗi từ server
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse>(errorContent);
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        throw new Exception(errorResponse.Message);
                    }
                }
                catch (JsonException)
                {
                    // Nếu không parse được JSON
                }
                
                return false;
            }
        }

        
    }
}
